using MR.Net.Frame;
using MR.Net.Proto.Battle;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRoom : MonoBehaviour {
    private static string s_SelfName;

    public TestRoomItemSeat[] seats;

    public GameObject loginGroup;
    public TMP_Dropdown dpdChannel;
    public Button btnLogin;
    public TMP_InputField ifdName;

    public GameObject roomGroup;
    public Button btnExit;
    public Button btnStand;
    public Button btnReady;
    public Button btnUnReady;

    public TMP_Text txtTime;

    private TcpInstance m_Tcp;
    private List<Player> m_Players = new List<Player>();

    private Player m_Self;

    private int m_ReadyNum;
    private float m_WaitTime;
    private bool m_Gaming;

    private void Start() {
        ifdName.text = s_SelfName;
        for (int i = 0; i < Net.Channels.Count; i++)
            dpdChannel.options.Add(new TMP_Dropdown.OptionData(Net.Channels[i].name));
        dpdChannel.value = PlayerPrefs.GetInt("ChannelIndex");
        dpdChannel.onValueChanged.AddListener(OnChannelChange);
        btnLogin.onClick.AddListener(OnLogin);
        btnExit.onClick.AddListener(OnExit);
        btnStand.onClick.AddListener(OnStand);
        btnReady.onClick.AddListener(OnReady);
        btnUnReady.onClick.AddListener(OnUnReady);
        for (int i = 0; i < seats.Length; i++)
            seats[i].Init(this, i);
        m_Tcp = Net.GetTcp();
        if (m_Tcp == null) {
            loginGroup.SetActive(true);
            roomGroup.SetActive(false);
        } else {
            loginGroup.SetActive(false);
            roomGroup.SetActive(true);
            Regist();
        }
    }

    private void OnChannelChange(int index) {
        PlayerPrefs.SetInt("ChannelIndex", index);
    }

    private void Update() {
        if (!m_Gaming) {
            var allReady = true;
            var rNum = 0;
            for (int i = 0; i < m_Players.Count; i++) {
                var player = m_Players[i];
                if (player.seat != -1) {
                    rNum++;
                    if (!player.ready) {
                        allReady = false;
                        break;
                    }
                }
            }
            if (rNum > 0 && allReady) {
                if (m_ReadyNum != rNum) {
                    m_WaitTime = 5;
                    m_ReadyNum = rNum;
                } else
                    m_WaitTime -= Time.deltaTime;
            } else {
                m_ReadyNum = 0;
                m_WaitTime = 0;
            }
        } else {
            m_WaitTime -= Time.deltaTime;
        }
        if (m_WaitTime > 0)
            txtTime.text = m_WaitTime.ToString("0");
        else
            txtTime.text = "";
    }

    private void OnLogin() {
        loginGroup.SetActive(false);
        Net.ChannelIdx = dpdChannel.value;
        m_Tcp = Net.CreateTcp();
        s_SelfName = ifdName.text;
        //m_Tcp.Send(new EnterRoomC2S { Name = s_SelfName }, (EnterRoomS2C msg) => {
        //    if (msg.Code != CodePBType.Success) {
        //        Debug.Log(msg.Code);
        //        m_Tcp.Close();
        //        loginGroup.SetActive(true);
        //    } else {
        //        InRoom();
        //    }
        //});
        Regist();
    }

    private void OnDestroy() {
        if (m_Tcp != null)
            UnRegist();
    }

    private void Regist() {
        m_Tcp.Regist<RoomDataS2C>(OnMsgRoomData);
        m_Tcp.Regist<PlayerUpdateS2C>(OnMsgPlayerUpdate);
        m_Tcp.Regist<GameStartS2C>(OnMsgGameStart);
    }

    private void UnRegist() {
        m_Tcp.UnRegist<RoomDataS2C>(OnMsgRoomData);
        m_Tcp.UnRegist<PlayerUpdateS2C>(OnMsgPlayerUpdate);
        m_Tcp.UnRegist<GameStartS2C>(OnMsgGameStart);
    }

    private void OnExit() {
        roomGroup.SetActive(false);
        m_Tcp.Send(new ExitRoomC2S(), (ExitRoomS2C msg) => {
            if (msg.Code != CodePBType.Success) {
                Debug.Log(msg.Code);
                roomGroup.SetActive(true);
            } else {
                OutRoom();
                UnRegist();
                m_Tcp.Close();
                m_Tcp = null;
            }
        });
    }

    private void InRoom() {
        roomGroup.SetActive(true);

    }

    private void OutRoom() {
        loginGroup.SetActive(true);
    }

    private void InitPlayer(PlayerPB[] players) {
        m_Players.Clear();
        foreach (var data in players) {
            var player = new Player { name = data.Name, seat = data.Seat, ready = data.Ready };
            m_Players.Add(player);
            if (s_SelfName == player.name)
                m_Self = player;
        }
        UpdateSeats();
    }

    private void UpdateSeats() {
        for (int i = 0; i < seats.Length; i++) {
            var player = m_Players.Find(m => m.seat == i);
            seats[i].UpdateData(player, m_Self.seat != -1, player == m_Self);
        }
        btnStand.gameObject.SetActive(m_Self.seat != -1);
        btnReady.gameObject.SetActive(m_Self.seat != -1 && !m_Self.ready);
        btnUnReady.gameObject.SetActive(m_Self.seat != -1 && m_Self.ready);
    }

    public class Player {
        public string name;
        public int seat;
        public bool ready;
    }

    public void OnStand() {
        m_Tcp.Send(new StandUpC2S(), (StandUpS2C msg) => {
            Debug.Log(msg.Code);
        });
    }

    public void OnReady() {
        m_Tcp.Send(new ReadyC2S { Value = true }, (ReadyS2C msg) => {
            Debug.Log(msg.Code);
        });
    }

    public void OnUnReady() {
        m_Tcp.Send(new ReadyC2S { Value = false }, (ReadyS2C msg) => {
            Debug.Log(msg.Code);
        });
    }

    public void OnSit(int seat) {
        m_Tcp.Send(new SitDownC2S { Seat = seat }, (SitDownS2C msg) => {
            Debug.Log(msg.Code);
        });
    }

    public void OnMove(int seat) {
        m_Tcp.Send(new MoveC2S { Seat = seat }, (MoveS2C msg) => {
            Debug.Log(msg.Code);
        });
    }

    private void OnMsgRoomData(RoomDataS2C msg) {
        InitPlayer(msg.RoomData.Players);
    }

    private void OnMsgPlayerUpdate(PlayerUpdateS2C msg) {
        switch (msg.Mode) {
            case UpdatePBType.Add:
                m_Players.Add(new Player { name = msg.Player.Name, seat = msg.Player.Seat });
                break;
            case UpdatePBType.Remove:
                m_Players.RemoveAll(m => m.name == msg.Player.Name);
                break;
            case UpdatePBType.Update:
                var target = m_Players.Find(m => m.name == msg.Player.Name);
                target.seat = msg.Player.Seat;
                target.ready = msg.Player.Ready;
                break;
        }
        UpdateSeats();
    }

    private void OnMsgGameStart(GameStartS2C msg) {
        TestBattle.StartMsg = msg;
        var dic = new Dictionary<int, string>();
        foreach (var player in m_Players)
            dic.Add(player.seat, player.name);
        TestBattle.PlayerNameDic = dic;
        PlayerPrefs.SetInt("Test", 1);
        Main.Instance.OpenUIStage("RoomTestBattle");
    }
}
