using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("并且")]
    public class EAnd : EAPIReturn<bool> {
        [LabelText("条件A")]
        public EAPIReturn<bool> condA;
        [LabelText("条件B")]
        public EAPIReturn<bool> condB;
        public override string ToString() => GetName(condA) + "并且" + GetName(condB);
    }
}
