using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("数组创建")]
    public class ECreateArray<T> : EAPIAct {
        [HideLabel]
        public EArray<T> array = new EArray<T>();
        [LabelText("初始值")]
        public EAPIReturnArray<T> value;
        public override string ToString() => string.IsNullOrEmpty(array.ToString()) ? $"创建数组{GetName(array)}" : $"创建数组{GetName(array)}={GetName(value)}";
    }
}
