using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 1, 0)]
    public class ConditionBoxDrawer : OdinValueDrawer<ConditionBox> {


        protected override void DrawPropertyLayout(GUIContent label) {
            if (ValueEntry.SmartValue == null)
                ValueEntry.SmartValue = new ConditionBox();
            bool open = (bool)Property.Children[0].ValueEntry.WeakSmartValue;
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            Rect controlRect = EditorGUILayout.GetControlRect(false);
            Property.Children[0].ValueEntry.WeakSmartValue = EditorGUI.Toggle(controlRect, label, open);
            SirenixEditorGUI.EndBoxHeader();
            if (SirenixEditorGUI.BeginFadeGroup(this, open)) {
                GameEditorGUI.BeginBox();
                if (Property.Children.Count > 0)
                    Property.Children[1].Draw();
                GameEditorGUI.EndBox();
            }
            SirenixEditorGUI.EndFadeGroup();
            SirenixEditorGUI.EndBox();
        }
    }
}
