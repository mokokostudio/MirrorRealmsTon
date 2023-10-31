
using System.Collections.Generic;
public static partial class Config {
    public static partial class Global {
        internal static void Load(Loader loader) {
            loader.Dispose();
        }
    }
}