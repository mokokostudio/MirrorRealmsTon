using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("静态")]
    public class EStatic<T> : EAPIReturn<T> {
        [HideLabel]
        public T value;

        public override string ToString() => GetName(value);
    }
}
