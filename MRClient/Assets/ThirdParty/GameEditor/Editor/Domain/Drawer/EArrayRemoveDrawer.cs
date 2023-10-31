using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class EArrayRemoveDrawer<T> : OdinValueDrawer<EArrayRemove<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            if (!DomainUtil.CheckVariable(ValueEntry.SmartValue.array))
                ValueEntry.SmartValue.array = null;
            CallNextDrawer(label);
        }
    }
}
