using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("或者")]
    public class EOR : EAPIReturn<bool> {
        [LabelText("条件A")]
        public EAPIReturn<bool> condA;
        [LabelText("条件B")]
        public EAPIReturn<bool> condB;
        public override string ToString() => GetName(condA) + "或者" + GetName(condB);
    }
}
