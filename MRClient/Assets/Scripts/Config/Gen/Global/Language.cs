
public static partial class Config {
    public static partial class Global {
        public class LanguageData {
            public LanReference BlackBoard { get; private set; }
            public LanReference Student { get; private set; }
            public LanReference Teacher { get; private set; }
            internal LanguageData(Loader loader) {
                BlackBoard = loader.ReadInt();
                Student = loader.ReadInt();
                Teacher = loader.ReadInt();
            }
        }
    }
}