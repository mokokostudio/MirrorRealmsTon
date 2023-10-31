
public static partial class Config {
    public static partial class School {
        public class Results {
            public LessonType Lesson { get; private set; }
            public int Value { get; private set; }
            internal Results(Loader loader) {
                Lesson = (LessonType)loader.ReadInt();
                Value = loader.ReadInt();
            }
        }
    }
}