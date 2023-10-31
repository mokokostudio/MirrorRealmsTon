using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class ESetVariableDrawer<T> : OdinValueDrawer<ESetVariable<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            if (!DomainUtil.CheckVariable(ValueEntry.SmartValue.variable))
                ValueEntry.SmartValue.variable = null;
            CallNextDrawer(label);
        }
    }
}
