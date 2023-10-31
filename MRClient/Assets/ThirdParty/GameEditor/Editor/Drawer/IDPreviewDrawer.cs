using GameEditor.Editors;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {

    public class IDPreviewDrawer : OdinAttributeDrawer<IDPreviewAttribute, int> {
        protected override void DrawPropertyLayout(GUIContent label) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            CallNextDrawer(label);
            EditorGUILayout.EndVertical();
            var enable = GUI.enabled;
            GUI.enabled = true;
            if (GameEditorGUI.InlineButton(GameEditorGUI.GetIcon("copy"), "复制"))
                GUIUtility.systemCopyBuffer = ValueEntry.SmartValue.ToString();
            GUI.enabled = enable;
            EditorGUILayout.EndHorizontal();
        }
    }
}
