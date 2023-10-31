using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace GameEditor.Editors.Domain {

    [DrawerPriority(1, 0, 0)]
    public class DomainAssetDrawer : OdinValueDrawer<DomainAsset> {
        protected override void DrawPropertyLayout(GUIContent label) {
            GameEditorGUI.BeginBox();
            CallNextDrawer(label);
            GameEditorGUI.EndBox();
        }
    }
}
