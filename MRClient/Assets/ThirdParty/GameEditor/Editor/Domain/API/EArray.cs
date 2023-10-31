using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [HideDuplicateReferenceBox]
    public class EArray<T> : EAPIReturnArray<T>, IVariable {
        [LabelText("名字")]
        [ShowInInspector]
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
