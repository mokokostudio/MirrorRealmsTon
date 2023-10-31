using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("判定相同")]
    public class EEquate<T> : EAPIReturn<bool> {
        [LabelText("值A")]
        public EAPIReturn<T> valueA;
        [LabelText("值B")]
        public EAPIReturn<T> valueB;
        public override string ToString() => $"{GetName(valueA)}与{GetName(valueB)}相同";
    }
}
