using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("数组添加")]
    public class EArrayAdd<T> : EAPIAct {
        public EArrayAdd() { }
        public EArrayAdd(EArray<T> v) => array = v;
        [LabelText("数组")]
        public EArray<T> array;
        [LabelText("目标")]
        public EAPIReturn<T> target;
        public override string ToString() => $"为数组{GetName(array)}添加{GetName(target)}";
    }
}
