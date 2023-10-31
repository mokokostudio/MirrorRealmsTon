using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("变量创建")]
    public class ECreateVariable<T> : EAPIAct {
        [HideLabel]
        public EVariable<T> variable = new EVariable<T>();
        [LabelText("初始值")]
        public EAPIReturn<T> value;
        public override string ToString() => string.IsNullOrEmpty(variable?.Name) ? $"创建变量{GetName(variable)}" : $"创建变量{GetName(variable)}={GetName(value)}";
    }
}
