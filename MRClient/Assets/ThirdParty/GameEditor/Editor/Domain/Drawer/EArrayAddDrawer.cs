using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class EArrayAddDrawer<T> : OdinValueDrawer<EArrayAdd<T>> {
        protected override void DrawPropertyLayout(GUIContent label) {
            if (!DomainUtil.CheckVariable(ValueEntry.SmartValue.array))
                ValueEntry.SmartValue.array = null;
            CallNextDrawer(label);
        }
    }
}
