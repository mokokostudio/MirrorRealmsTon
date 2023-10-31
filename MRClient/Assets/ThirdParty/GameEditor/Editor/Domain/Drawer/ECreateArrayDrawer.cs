using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class ECreateArrayDrawer<T> : OdinValueDrawer<ECreateArray<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            DomainUtil.CheckVariableCreate(ValueEntry.SmartValue.array);
            CallNextDrawer(label);
            DomainUtil.AddVariable(ValueEntry.SmartValue.array);
        }
    }
}
