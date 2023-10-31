
using System.Collections.Generic;
public static partial class Config {
    public static partial class Equips {
        public class WeaponData {
            public ulong ID { get; private set; }
            public ulong Type { get; private set; }
            public Global.QualityType Quality { get; private set; }
            public LanReference Name { get; private set; }
            public string Icon { get; private set; }
            public List<WeaponCardType> Cards { get; private set; }
            public List<string> Prefabs { get; private set; }
            public LanReference Info { get; private set; }
            internal WeaponData(Loader loader) {
                ID = loader.ReadULong();
                Type = loader.ReadULong();
                Quality = (Global.QualityType)loader.ReadInt();
                Name = loader.ReadInt();
                Icon = loader.ReadString();
                Cards = loader.ReadArray(() => (WeaponCardType)loader.ReadInt());
                Prefabs = loader.ReadArray(loader.ReadString);
                Info = loader.ReadInt();
            }
        }
    }
}