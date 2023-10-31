using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("数组移除")]
    public class EArrayRemove<T> : EAPIAct {
        public EArrayRemove() { }
        public EArrayRemove(EArray<T> v) => array = v;
        [LabelText("数组")]
        public EArray<T> array;
        [LabelText("目标")]
        public EAPIReturn<T> target;
        public override string ToString() => $"从数组{GetName(array)}移除{GetName(target)}";
    }
}
