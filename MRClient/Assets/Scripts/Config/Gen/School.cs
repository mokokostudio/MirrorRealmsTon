
using System.Collections.Generic;
public static partial class Config {
    public static partial class School {
        public static Dictionary<ulong, ClassData> Class { get; private set; }
        public static Dictionary<ulong, StudentData> Student { get; private set; }
        public static Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, TestData>>>> Test { get; private set; }
        internal static void Load(Loader loader) {
            Class = Turn(loader.ReadArray(() => new ClassData(loader)), m => m.ID);
            Student = Turn(loader.ReadArray(() => new StudentData(loader)), m => m.ID);
            Test = Turn(loader.ReadArray(() => new TestData(loader)), m => m.N1, l => Turn(l, m => m.N2, l => Turn(l, m => m.N3, l => Turn(l, m => m.N4))));
            loader.Dispose();
        }
    }
}