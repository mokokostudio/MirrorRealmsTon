using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [HideDuplicateReferenceBox]
    public class EVariable<T> : EAPIReturn<T>, IVariable {
        [LabelText("名字")]
        [ShowInInspector]
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
