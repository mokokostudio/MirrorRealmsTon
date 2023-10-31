
using System.Collections.Generic;
public static partial class Config {
    public static partial class Equips {
        public static Dictionary<ulong, WeaponData> Weapon { get; private set; }
        public static Dictionary<ulong, WeaponTypeData> WeaponType { get; private set; }
        internal static void Load(Loader loader) {
            Weapon = Turn(loader.ReadArray(() => new WeaponData(loader)), m => m.ID);
            WeaponType = Turn(loader.ReadArray(() => new WeaponTypeData(loader)), m => m.ID);
            loader.Dispose();
        }
    }
}