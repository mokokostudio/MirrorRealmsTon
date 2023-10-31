
using System.Collections.Generic;
public static partial class Config {
    public static partial class School {
        public class ClassData {
            public ulong ID { get; private set; }
            public LanReference Name { get; private set; }
            internal ClassData(Loader loader) {
                ID = loader.ReadULong();
                Name = loader.ReadInt();
            }
        }
    }
}