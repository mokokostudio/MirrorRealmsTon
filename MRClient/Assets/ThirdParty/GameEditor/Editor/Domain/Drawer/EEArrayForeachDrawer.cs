using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class EEArrayForeachDrawer<T> : OdinValueDrawer<EArrayForeach<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            DomainUtil.CheckVariableCreate(ValueEntry.SmartValue.item);
            CallNextDrawer(label);
        }
    }
}
