
using System;
public static partial class Config {
    public static void LoadData(Func<string, byte[]> dataGet) {
        Battle.Load(new Loader(dataGet("Battle")));
        Equips.Load(new Loader(dataGet("Equips")));
        Global.Load(new Loader(dataGet("Global")));
    }
}