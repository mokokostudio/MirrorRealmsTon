
using System.Collections.Generic;
public static partial class Config {
    public static partial class School {
        public class TestData {
            public int N1 { get; private set; }
            public int N2 { get; private set; }
            public int N3 { get; private set; }
            public int N4 { get; private set; }
            public LanReference Name { get; private set; }
            internal TestData(Loader loader) {
                N1 = loader.ReadInt();
                N2 = loader.ReadInt();
                N3 = loader.ReadInt();
                N4 = loader.ReadInt();
                Name = loader.ReadInt();
            }
        }
    }
}