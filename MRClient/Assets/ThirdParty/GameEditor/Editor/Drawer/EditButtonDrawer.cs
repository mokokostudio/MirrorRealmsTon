using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public class EditButtonDrawer : OdinAttributeDrawer<EditButtonAttribute> {
        private ValueResolver<GameEditorAsset> m_Resolver;
        protected override void Initialize() {
            m_Resolver = ValueResolver.Get<GameEditorAsset>(Property, Attribute.asset);
        }
        protected override void DrawPropertyLayout(GUIContent label) {
            EditorGUILayout.BeginHorizontal();
            CallNextDrawer(label);
            var target = m_Resolver.GetValue();
            bool f = GUI.enabled;
            GUI.enabled = target != null;
            if (GameEditorGUI.InlineButton(GameEditorGUI.GetIcon("edit"), "±à¼­"))
                GameEditorWindow.Current?.Select(target);
            GUI.enabled = f;
            EditorGUILayout.EndHorizontal();
        }
    }
}