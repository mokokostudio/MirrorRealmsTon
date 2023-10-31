
using System.Collections.Generic;
public static partial class Config {
    public static partial class School {
        public class StudentData {
            public ulong ID { get; private set; }
            public ulong Class { get; private set; }
            public LanReference Name { get; private set; }
            public List<Results> Score { get; private set; }
            public List<Results> Results { get; private set; }
            internal StudentData(Loader loader) {
                ID = loader.ReadULong();
                Class = loader.ReadULong();
                Name = loader.ReadInt();
                Score = loader.ReadArray(() => new Results(loader));
                Results = loader.ReadArray(() => new Results(loader));
            }
        }
    }
}