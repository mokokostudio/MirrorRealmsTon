using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 1, 0)]
    public class EAPIActListDrawer : OdinValueDrawer<List<EAPIAct>> {
        private int m_DragToIndex = -1;

        protected override void DrawPropertyLayout(GUIContent label) {
            var dragToIndex = m_DragToIndex;
            DomainUtil.VariableBegin();
            var dh = DragAndDropManager.BeginDropZone<EAPIAct>(Property, true);
            if (dh.IsReadyToClaim && Event.current.type == EventType.Repaint) {
                DomainUtil.dropToList = ValueEntry.SmartValue;
                var obj = dh.ClaimObject();
                var resolver = Property.ChildResolver as IOrderedCollectionResolver;

                if (ValueEntry.SmartValue.Contains(obj as EAPIAct)) {
                    int idx = ValueEntry.SmartValue.IndexOf(obj as EAPIAct);
                    resolver.QueueRemove(obj, 0);
                    if (idx < m_DragToIndex)
                        m_DragToIndex--;
                }

                if (m_DragToIndex == -1)
                    resolver.QueueAdd(obj, 0);
                else
                    resolver.QueueInsertAt(m_DragToIndex, obj, 0);
                m_DragToIndex = -1;
            }

            var rect1 = EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("plus")))
                DomainUtil.ShowTypeSelection(typeof(EAPIAct), rect1, null, m => {
                    ValueEntry.SmartValue.Add(m as EAPIAct);
                    Property.SetDirty();
                });
            EditorGUILayout.EndHorizontal();
            if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
                m_DragToIndex = -1;

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < Property.Children.Count; i++) {
                var rect2 = EditorGUILayout.BeginVertical();
                if (dh.IsBeingHovered && (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform))
                    if (rect2.Contains(Event.current.mousePosition)) {
                        m_DragToIndex = i;
                    }
                if (dragToIndex == i)
                    GameEditorGUI.DrawLine(Color.white);
                else
                    GameEditorGUI.DrawLine();
                Property.Children[i].Draw();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (dh.IsBeingHovered && dragToIndex == -1)
                GameEditorGUI.DrawLine(Color.white);

            DragAndDropManager.EndDropZone();
            DomainUtil.VariableEnd();

            GameEditorGUI.AddRightClickArea(Property.LastDrawnValueRect, () => {
                var pr = Property.ChildResolver as IOrderedCollectionResolver;
                var options = new GameEditorGUI.Options();
                options.AddOption("复制列表", () => Clipboard.Copy(ValueEntry.SmartValue), Property.Children.Count > 0);
                options.AddOption("粘贴列表", () => ValueEntry.SmartValue = Clipboard.Paste<List<EAPIAct>>(), Clipboard.CanPaste<List<EAPIAct>>());
                options.AddOption("");
                options.AddOption("清空列表", () => pr.QueueClear(), Property.Children.Count > 0);
                return options.MakeGenericMenu();
            });
        }
    }
}
