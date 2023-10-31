
using System.Collections.Generic;
public static partial class Config {
    public static partial class Battle {
        public class ConstantData {
            public int LockMissDistance { get; private set; }
            public int MaxLockDistance { get; private set; }
            public int MaxLockAngle { get; private set; }
            public int DualDuration { get; private set; }
            public int SPMax { get; private set; }
            public int SPGrowBase { get; private set; }
            public int SPGrowFromWeapon { get; private set; }
            public int SPDeductRate { get; private set; }
            public int SPGrowbyDefense { get; private set; }
            public int EPMax { get; private set; }
            public int EpReductByDodge { get; private set; }
            public int EpRecoverTime { get; private set; }
            public int HPMax { get; private set; }
            internal ConstantData(Loader loader) {
                LockMissDistance = loader.ReadInt();
                MaxLockDistance = loader.ReadInt();
                MaxLockAngle = loader.ReadInt();
                DualDuration = loader.ReadInt();
                SPMax = loader.ReadInt();
                SPGrowBase = loader.ReadInt();
                SPGrowFromWeapon = loader.ReadInt();
                SPDeductRate = loader.ReadInt();
                SPGrowbyDefense = loader.ReadInt();
                EPMax = loader.ReadInt();
                EpReductByDodge = loader.ReadInt();
                EpRecoverTime = loader.ReadInt();
                HPMax = loader.ReadInt();
            }
        }
    }
}