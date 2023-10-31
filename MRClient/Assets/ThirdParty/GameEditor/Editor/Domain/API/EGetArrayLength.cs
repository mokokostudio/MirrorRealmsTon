using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("获取数组长度")]
    public class EGetArrayLength<T> : EAPIReturn<float> {
        public EGetArrayLength() { }
        public EGetArrayLength(EArray<T> v) => array = v;
        [LabelText("数组")]
        public EArray<T> array;

        public override string ToString() => $"获取数组 {GetName(array)} 长度";
    }
}