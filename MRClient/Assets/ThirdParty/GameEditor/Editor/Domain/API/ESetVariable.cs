using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("变量设置")]
    public class ESetVariable<T> : EAPIAct {
        public ESetVariable() { }
        public ESetVariable(EVariable<T> v = null) => variable = v;
        [LabelText("变量")]
        public EVariable<T> variable;
        [LabelText("值")]
        public EAPIReturn<T> value;
        public override string ToString() => variable == null ? $"设置{GetName(variable)}" : $"设置{GetName(variable)}={GetName(value)}";
    }
}
