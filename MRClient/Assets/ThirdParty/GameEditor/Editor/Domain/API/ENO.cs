using Sirenix.OdinInspector;

namespace GameEditor.Editors.Domain {
    [APIName("取反")]
    public class ENO : EAPIReturn<bool> {
        [LabelText("条件")]
        public EAPIReturn<bool> cond;
        public override string ToString() => $"{GetName(cond)}不成立";
    }
}
