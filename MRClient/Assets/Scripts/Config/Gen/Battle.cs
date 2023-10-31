
using System.Collections.Generic;
public static partial class Config {
    public static partial class Battle {
        public static ConstantData Constant { get; private set; }
        internal static void Load(Loader loader) {
            Constant = new ConstantData(loader);
            loader.Dispose();
        }
    }
}