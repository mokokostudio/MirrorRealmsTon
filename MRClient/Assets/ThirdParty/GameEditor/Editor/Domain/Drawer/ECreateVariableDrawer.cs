using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class ECreateVariableDrawer<T> : OdinValueDrawer<ECreateVariable<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            DomainUtil.CheckVariableCreate(ValueEntry.SmartValue.variable);
            CallNextDrawer(label);
            DomainUtil.AddVariable(ValueEntry.SmartValue.variable);
        }
    }
}
