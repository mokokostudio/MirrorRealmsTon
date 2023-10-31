using ProtoBuf;

namespace MR.Net.Proto.Battle {

    [ProtoMessage(200001)]
    [ProtoContract]
    public class LoginC2S {
        [ProtoMember(1)]
        public string Account { get; set; }
        [ProtoMember(2)]
        public string Password { get; set; }
    }

    [ProtoMessage(201001)]
    [ProtoContract]
    public class LoginS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
        [ProtoMember(2)]
        public UserInfo UserInfo { get; set; }
    }

    [ProtoMessage(200002)]
    [ProtoContract]
    public class ChangeEquipC2S {
        [ProtoMember(1)]
        public int WeaponIndex { get; set; }
        [ProtoMember(2)]
        public int LocationIndex { get; set; }
    }

    [ProtoMessage(201002)]
    [ProtoContract]
    public class ChangeEquipS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
        [ProtoMember(2)]
        public int[] Equiped { get; set; }
    }

    [ProtoMessage(202001)]
    [ProtoContract]
    public class KickS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    //===========================================================

    [ProtoMessage(100001)]
    [ProtoContract]
    public class EnterRoomC2S {
        [ProtoMember(1)]
        public int RommID { get; set; }
    }

    [ProtoMessage(101001)]
    [ProtoContract]
    public class EnterRoomS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
        [ProtoMember(2)]
        public int RoomID { get; set; }
    }

    [ProtoMessage(100002)]
    [ProtoContract]
    public class ExitRoomC2S {
    }

    [ProtoMessage(101002)]
    [ProtoContract]
    public class ExitRoomS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100003)]
    [ProtoContract]
    public class SitDownC2S {
        [ProtoMember(1)]
        public int Seat { get; set; }
    }

    [ProtoMessage(101003)]
    [ProtoContract]
    public class SitDownS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100004)]
    [ProtoContract]
    public class StandUpC2S {
    }

    [ProtoMessage(101004)]
    [ProtoContract]
    public class StandUpS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100005)]
    [ProtoContract]
    public class MoveC2S {
        [ProtoMember(1)]
        public int Seat { get; set; }
    }

    [ProtoMessage(101005)]
    [ProtoContract]
    public class MoveS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100006)]
    [ProtoContract]
    public class ReadyC2S {
        [ProtoMember(1)]
        public bool Value { get; set; }
    }

    [ProtoMessage(101006)]
    [ProtoContract]
    public class ReadyS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100007)]
    [ProtoContract]
    public class CountC2S {
        [ProtoMember(1)]
        public bool Start { get; set; }
    }

    [ProtoMessage(101007)]
    [ProtoContract]
    public class CountS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(100008)]
    [ProtoContract]
    public class GameResultC2S {
        [ProtoMember(1)]
        public ScorePB[] ScorePBs { get; set; }
    }

    [ProtoMessage(101008)]
    [ProtoContract]
    public class GameResultS2C {
        [ProtoMember(1)]
        public CodePBType Code { get; set; }
    }

    [ProtoMessage(102001)]
    [ProtoContract]
    public class RoomDataS2C {
        [ProtoMember(1)]
        public RoomPB RoomData { get; set; }
    }

    [ProtoMessage(102002)]
    [ProtoContract]
    public class PlayerUpdateS2C {
        [ProtoMember(1)]
        public UpdatePBType Mode { get; set; }
        [ProtoMember(2)]
        public PlayerPB Player { get; set; }
    }

    [ProtoMessage(102003)]
    [ProtoContract]
    public class GameCountStartS2C {
        [ProtoMember(1)]
        public int Scend { get; set; }
    }

    [ProtoMessage(102004)]
    [ProtoContract]
    public class GameCountCancelS2C {
    }

    [ProtoMessage(102005)]
    [ProtoContract]
    public class GameStartS2C {
        [ProtoMember(1)]
        public int Index { get; set; }
        [ProtoMember(2)]
        public int Token { get; set; }
        [ProtoMember(3)]
        public int[] PlayerIndexs { get; set; }
        [ProtoMember(4)]
        public int Sead { get; set; }
        [ProtoMember(5)]
        public int Port { get; set; }
        [ProtoMember(6)]
        public int GameTime { get; set; }
        [ProtoMember(7)]
        public int GameDelayTime { get; set; }
    }

    [ProtoMessage(102006)]
    [ProtoContract]
    public class GameEndS2C {
        [ProtoMember(1)]
        public bool Sync { get; set; }
    }

    [ProtoMessage(102007)]
    [ProtoContract]
    public class GameRunS2C {
    }

    [ProtoMessage(102008)]
    [ProtoContract]
    public class RoomBreakupS2C {
    }

    [ProtoContract]
    public enum CodePBType {
        [ProtoEnum]
        Success = 0,
        [ProtoEnum]
        AllreadyInRooom = 1,
        [ProtoEnum]
        GameInProgress = 2,
        [ProtoEnum]
        DuplicateName = 3,
        [ProtoEnum]
        SeatOccupied = 4,
        [ProtoEnum]
        ReadyNeadSit = 5,
        [ProtoEnum]
        ReadyCanNotMove = 6,
        [ProtoEnum]
        RoomNotExist = 7,
        [ProtoEnum]
        NotAllReady = 8,
        [ProtoEnum]
        NoPermission = 9,
        [ProtoEnum]
        GameInCount = 10,
        [ProtoEnum]
        PlayerNumberError = 11,
        [ProtoEnum]
        GameNotBegin = 12,
        [ProtoEnum]
        VerifyFailed = 13,

        [ProtoEnum]
        ParamError = 8000,

        [ProtoEnum]
        RepeatLogin = 9000,
    }

    [ProtoContract]
    public enum UpdatePBType {
        [ProtoEnum]
        None = 0,
        [ProtoEnum]
        Add = 1,
        [ProtoEnum]
        Remove = 2,
        [ProtoEnum]
        Update = 3
    }

    [ProtoContract]
    public class RoomPB {
        [ProtoMember(1)]
        public PlayerPB[] Players { get; set; }
    }

    [ProtoContract]
    public class PlayerPB {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int Seat { get; set; }
        [ProtoMember(3)]
        public bool Ready { get; set; }
        [ProtoMember(4)]
        public ulong Character { get; set; }
        [ProtoMember(5)]
        public ulong[] Weapons { get; set; }
    }

    [ProtoContract]
    public class ScorePB {
        [ProtoMember(1)]
        public int Index { get; set; }
        [ProtoMember(2)]
        public int Kill { get; set; }
        [ProtoMember(3)]
        public int Die { get; set; }
        [ProtoMember(4)]
        public long Output { get; set; }
        [ProtoMember(5)]
        public long Take { get; set; }
    }

    [ProtoContract]
    public class UserInfo {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public WeaponPB[] Weapons { get; set; }
        [ProtoMember(3)]
        public int[] Equiped { get; set; }
    }

    [ProtoContract]
    public class WeaponPB {
        [ProtoMember(1)]
        public int ID { get; set; }
        [ProtoMember(2)]
        public ulong ConfigID { get; set; }
        [ProtoMember(3)]
        public int Quality { get; set; }
        [ProtoMember(4)]
        public string Prop1 { get; set; }
        [ProtoMember(5)]
        public string Prop2 { get; set; }
    }
}
