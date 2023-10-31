
using System.Collections.Generic;
public static partial class Config {
    public static partial class Equips {
        public class WeaponTypeData {
            public ulong ID { get; private set; }
            public string Mode { get; private set; }
            public LanReference Des { get; private set; }
            public int CanDef { get; private set; }
            public List<int> EquipPosition { get; private set; }
            internal WeaponTypeData(Loader loader) {
                ID = loader.ReadULong();
                Mode = loader.ReadString();
                Des = loader.ReadInt();
                CanDef = loader.ReadInt();
                EquipPosition = loader.ReadArray(loader.ReadInt);
            }
        }
    }
}