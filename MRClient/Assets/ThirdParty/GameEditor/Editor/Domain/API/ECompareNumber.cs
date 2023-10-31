using GameEditor.Domain;
using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("数值比较")]
    public class ECompareNumber : EAPIReturn<bool> {
        [LabelText("数值A")]
        public EAPIReturn<float> numberA;
        [LabelText("比较类型")]
        public NumberCompareType compareType;
        [LabelText("数值B")]
        public EAPIReturn<float> numberB;
        public override string ToString() {
            string link;
            switch (compareType) {
                case NumberCompareType.Equal:
                    link = "==";
                    break;
                case NumberCompareType.NoEqual:
                    link = "!=";
                    break;
                case NumberCompareType.Greater:
                    link = ">";
                    break;
                case NumberCompareType.GreaterOrEqual:
                    link = ">=";
                    break;
                case NumberCompareType.Less:
                    link = "<";
                    break;
                case NumberCompareType.LessOrEqual:
                default:
                    link = "<=";
                    break;
            }
            return $"{GetName(numberA)}{GameEditorStyles.StaticColor.Dye(link)}{GetName(numberB)}";
        }
    }
}
