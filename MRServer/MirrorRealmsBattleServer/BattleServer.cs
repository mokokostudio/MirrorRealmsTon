using Microsoft.VisualBasic;
using MR.Net.Proto;
using MR.Net.Proto.Battle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MR.BattleServer {
    public class BattleServer {
        public static BattleServer Instance;

        private int m_UdpPortMin;
        private int m_updPortMax;

        private ConcurrentDictionary<int, Room> m_Rooms = new ConcurrentDictionary<int, Room>();
        private Random m_Random = new Random();

        private int m_UserNumber;
        private ConcurrentDictionary<string, User> m_Users = new ConcurrentDictionary<string, User>();

        #region 房间基础
        public BattleServer(int tcpPort, int udpPortMin, int updPortMax) {
            Instance = this;
            m_UdpPortMin = udpPortMin;
            m_updPortMax = updPortMax;
            BeginTcpListen(tcpPort);
            //AccountVerify("aureon@live.cn", GetMD5("52545658a"));
        }

        private async void BeginTcpListen(int port) {
            TcpListener listener = TcpListener.Create(port);
            listener.Start();
            Console.WriteLine($"StartListen TCP on {port}");
            while (true) {
                var client = await listener.AcceptTcpClientAsync();
                var handle = new TcpProtoHandle(client);
                handle.RegistOnce<LoginC2S>(OnLogin);
            }
        }

        private void OnLogin(TcpProtoHandle handle, LoginC2S msg) {
            AccountVerify(handle, msg.Account, msg.Password);
        }
        #endregion



        public void EnterRoom(TcpProtoHandle handle, int roomID, string name, ulong[] weapons) {
            if (roomID == 0) {
                var id = NewRoomID();
                var room = new Room(id, m_UdpPortMin, m_updPortMax);
                m_Rooms[id] = room;
                room.Enter(handle, name, weapons);
            } else {
                if (!m_Rooms.TryGetValue(roomID, out var room)) {
                    handle.Send(new EnterRoomS2C { Code = CodePBType.RoomNotExist });
                    return;
                }
                room.Enter(handle, name, weapons);
            }
        }

        private int NewRoomID() {
            int id;
            do {
                id = m_Random.Next(100000, 1000000);
            } while (m_Rooms.ContainsKey(id));
            return id;
        }

        public void RoomRelease(int id) {
            m_Rooms.TryRemove(id, out _);
        }

        public class User {
            public string Name { get; private set; }
            public WeaponData[] Weapons { get; private set; }
            public int[] Equiped { get; private set; }
            public TcpProtoHandle Handle { get; private set; }

            public UserInfo PB {
                get {
                    var result = new UserInfo();
                    result.Name = Name;
                    result.Weapons = new WeaponPB[Weapons.Length];
                    for (int i = 0; i < Weapons.Length; i++)
                        result.Weapons[i] = Weapons[i].PB;
                    result.Equiped = Equiped;
                    return result;
                }
            }

            public User(string name, TcpProtoHandle handle, List<WeaponData> weapons) {
                Name = name;
                RessetWeapon(weapons);
                Regist(handle);
            }

            private void RessetWeapon(List<WeaponData> weapons) {
                Weapons = new WeaponData[3 + weapons.Count];
                Weapons[0] = new WeaponData { configID = 1000001001 };
                Weapons[1] = new WeaponData { configID = 1000002001 };
                Weapons[2] = new WeaponData { configID = 1000003001 };
                Equiped = new int[] { 0, 1, 2 };
                for (int i = 0; i < weapons.Count; i++)
                    Weapons[i + 3] = weapons[i];
            }

            public void UpdateWeapon(List<WeaponData> weapons) {
                for (int i = 0; i < 3; i++) {
                    var weapon = Weapons[Equiped[i]];
                    if (weapon.id != 0 && !weapons.Exists(m => m.id == weapon.id)) {
                        RessetWeapon(weapons);
                        return;
                    }
                }
                for (int i = 0; i < Weapons.Length; i++) {
                    var weapon = Weapons[i];
                    if (weapon.id != 0) {
                        var hw = weapons.Find(m => m.id == weapon.id);
                        if (hw != null)
                            weapons.Remove(hw);
                    }
                }
                if (weapons.Count > 0) {
                    int len = Weapons.Length;
                    var array = new WeaponData[len + weapons.Count];
                    Array.Copy(Weapons, array, len);
                    Weapons = array;
                    for (int i = 0; i < weapons.Count; i++)
                        Weapons[i + len] = weapons[i];
                }
            }

            public void UpdateHandle(TcpProtoHandle handle) {
                if (Handle != null) {
                    Handle.Send(new KickS2C { Code = CodePBType.RepeatLogin });
                    UnRegist();
                }
                Regist(handle);
            }

            private void UnRegist() {
                Handle.UnRegist<ChangeEquipC2S>(OnCangeEquip);
                Handle.UnRegist<EnterRoomC2S>(OnEnterRoom);
                Handle.onClose -= UnRegist;
                Handle.Close();
                Handle = null;
            }

            private void Regist(TcpProtoHandle handle) {
                Handle = handle;
                Handle.Regist<ChangeEquipC2S>(OnCangeEquip);
                Handle.Regist<EnterRoomC2S>(OnEnterRoom);
                Handle.onClose += UnRegist;
            }

            private void OnCangeEquip(TcpProtoHandle handle, ChangeEquipC2S msg) {
                if (msg.LocationIndex < 0 || msg.LocationIndex > 2 || msg.WeaponIndex < 0 || msg.WeaponIndex >= Weapons.Length) {
                    handle.Send(new ChangeEquipS2C { Code = CodePBType.ParamError });
                    return;
                }

                if (Equiped[msg.LocationIndex] == msg.WeaponIndex) {
                    handle.Send(new ChangeEquipS2C { Code = CodePBType.Success, Equiped = Equiped });
                    return;
                }

                for (int i = 0; i < 3; i++)
                    if (i != msg.LocationIndex && Equiped[i] == msg.WeaponIndex)
                        Equiped[i] = Equiped[msg.LocationIndex];
                Equiped[msg.LocationIndex] = msg.WeaponIndex;
                handle.Send(new ChangeEquipS2C { Code = CodePBType.Success, Equiped = Equiped });
            }

            private void OnEnterRoom(TcpProtoHandle handle, EnterRoomC2S msg) {
                Instance.EnterRoom(handle, msg.RommID, Name, new ulong[] { Weapons[Equiped[0]].configID, Weapons[Equiped[1]].configID, Weapons[Equiped[2]].configID });
            }
        }

        public class WeaponData {
            public int id;
            public ulong configID;
            public int quality;
            public string prop1;
            public string prop2;

            public WeaponPB PB {
                get {
                    return new WeaponPB() {
                        ID = id,
                        ConfigID = configID,
                        Quality = quality,
                        Prop1 = prop1,
                        Prop2 = prop2,
                    };
                }
            }
        }

        private async Task AccountVerify(TcpProtoHandle handle, string account, string password) {
            var weapons = new List<WeaponData>();
            string nickName = null;
            try {
                using (var http = new HttpClient()) {
                    var content1 = new StringContent("{\"account\":\"" + account + "\",\"password\":\"" + password + "\"}", Encoding.UTF8, "application/json");
                    var rep1 = await http.PostAsync("mrbev1/LoginByPassword", content1);
                    var result1 = await rep1.Content.ReadAsStringAsync();
                    Console.WriteLine(result1);
                    JsonDocument doc1 = JsonDocument.Parse(result1);
                    var token = doc1.RootElement.GetProperty("token").GetString();
                    nickName = doc1.RootElement.GetProperty("account").GetProperty("nickname").GetString();

                    HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, "mrbev1/GetAptosNFTsV2");
                    msg.Content = new StringContent("{}");
                    msg.Headers.Add("Authorization", "Bearer " + token);
                    var rep2 = await http.SendAsync(msg);
                    var result2 = await rep2.Content.ReadAsStringAsync();
                    Console.WriteLine(result2);
                    JsonDocument doc2 = JsonDocument.Parse(result2);
                    if (doc2.RootElement.TryGetProperty("nfts", out var list)) {
                        var len = list.GetArrayLength();

                        for (int i = 0; i < len; i++) {
                            var node = list[i];
                            var weapon = new WeaponData();
                            weapon.id = node.GetProperty("token_id").GetInt32();
                            var prop = node.GetProperty("token_properties");
                            weapon.prop1 = prop.GetProperty("prop1").GetString();
                            weapon.prop2 = prop.GetProperty("prop2").GetString();
                            weapon.quality = int.Parse(prop.GetProperty("quality").GetString());
                            weapon.configID = ulong.Parse(prop.GetProperty("weapon_id").GetString());
                            weapons.Add(weapon);
                        }
                    }
                }
            } catch (Exception ex) {
                handle.Send(new LoginS2C { Code = CodePBType.VerifyFailed });
                Console.WriteLine(ex);
            }

            if (m_Users.TryGetValue(account, out var user)) {
                user.UpdateWeapon(weapons);
                user.UpdateHandle(handle);
            } else {
                user = new User(nickName, handle, weapons);
                m_Users.TryAdd(account, user);
            }
            handle.Send(new LoginS2C { Code = CodePBType.Success, UserInfo = user.PB });
        }
    }
}
