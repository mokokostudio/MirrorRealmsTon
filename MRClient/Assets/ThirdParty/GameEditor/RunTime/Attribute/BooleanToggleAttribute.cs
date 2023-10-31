using System;

namespace GameEditor {
    public class BooleanToggleAttribute : Attribute {
        public string trueName;
        public string falseName;
        public BooleanToggleAttribute() { }
        public BooleanToggleAttribute(string trueName,string falseName) {
            this.trueName = trueName;
            this.falseName = falseName;
        }
    }
}
