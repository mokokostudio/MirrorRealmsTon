using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class EAPIReturnDrawer<T, U> : OdinValueDrawer<U> where U : EAPIReturn<T> {
        protected override void DrawPropertyLayout(GUIContent label) {
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(7, false);
            if (Event.current.type == EventType.Repaint) {
                GUIHelper.PushColor(typeof(T).GetColor());
                GameEditorStyles.PrefixBlock.Draw(rect.AlignLeft(6), false, true, false, false);
                GUIHelper.PopColor();
            }
            EditorGUI.DrawRect(rect.AlignLeft(1).AddX(6), GameEditorStyles.BorderColor);
            EditorGUILayout.BeginVertical();
            CallNextDrawer(label);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GameEditorGUI.AddRightClickArea(Property.LastDrawnValueRect, () => {
                var options = new GameEditorGUI.Options();
                options.AddOption("复制", () => Clipboard.Copy(ValueEntry.SmartValue), ValueEntry.SmartValue != null);
                options.AddOption("粘贴", () => ValueEntry.WeakSmartValue = Clipboard.Paste<EAPIReturn<T>>(), Clipboard.CanPaste<EAPIReturn<T>>());
                return options.MakeGenericMenu();
            });
        }
    }
}
