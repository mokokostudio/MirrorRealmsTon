using System;

namespace GameEditor.Editors {
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetConfigAttribute : Attribute {
        public string Name;
        public string Icon;
        public string Path;
    }

    public class NumberRangeAttribute : Attribute {
        public int Min;
        public int Max;
        private string format;
        public string Format {
            get {
                if (format == null) {
                    format = "";
                    int n = Max;
                    while (n > 0) {
                        n /= 10;
                        format += '0';
                    }
                }
                return format;
            }
        }

        public NumberRangeAttribute() {
            Min = 1;
            Max = 9999;
        }

        public NumberRangeAttribute(int min, int max) {
            Min = min;
            Max = max;
        }
    }

    public class IDRuleAttribute : Attribute {
        public string Parse;

        public IDRuleAttribute() { }
        public IDRuleAttribute(string parse) {
            Parse = parse;
        }
    }
}
