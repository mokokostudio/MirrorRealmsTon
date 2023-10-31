
using System.Collections.Generic;
public static partial class Config {
    public static partial class Global {
        public class ConstantData {
            public int AverageHeight { get; private set; }
            public int AverageWeight { get; private set; }
            public List<ulong> BestStudents { get; private set; }
            public List<School.Results> Results { get; private set; }
            internal ConstantData(Loader loader) {
                AverageHeight = loader.ReadInt();
                AverageWeight = loader.ReadInt();
                BestStudents = loader.ReadArray(loader.ReadULong);
                Results = loader.ReadArray(() => new School.Results(loader));
            }
        }
    }
}