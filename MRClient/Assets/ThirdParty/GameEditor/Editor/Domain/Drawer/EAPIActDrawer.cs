using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 2, 0)]
    public class EAPIActDrawer<T> : OdinValueDrawer<T> where T : EAPIAct {
        protected override void DrawPropertyLayout(GUIContent label) {

            var dragHandle = DragAndDropManager.BeginDragHandle(Property, ValueEntry.SmartValue);

            if (dragHandle.OnDragStarted) {
                dragHandle.OnDragFinnished = dropEvent => {
                    if (dropEvent == DropEvents.Moved && !DomainUtil.dropToList.Contains(ValueEntry.SmartValue)) {
                        var resolver = Property.Parent.ChildResolver as IOrderedCollectionResolver;
                        resolver.QueueRemove(ValueEntry.SmartValue, 0);
                    }
                };
            }

            if (DragAndDropManager.CurrentDraggingHandle == dragHandle) {
                DragAndDropManager.AllowDrop = false;
                var rect = Draw(label, dragHandle);
                DragAndDropManager.AllowDrop = true;
                EditorGUI.DrawRect(rect, GameEditorStyles.MaskColor);
            } else {
                Draw(label, dragHandle);
            }

            DragAndDropManager.EndDragHandle();

            GameEditorGUI.AddRightClickArea(Property.LastDrawnValueRect, () => {
                var pr = Property.Parent.ChildResolver as IOrderedCollectionResolver;
                var options = new GameEditorGUI.Options();
                options.AddOption("复制", () => Clipboard.Copy(ValueEntry.SmartValue), ValueEntry.SmartValue != null);
                options.AddOption("粘贴", () => pr.QueueInsertAt(Property.Index, Clipboard.Paste<EAPIAct>(), 0), Clipboard.CanPaste<EAPIAct>());
                options.AddOption("");
                options.AddOption("删除", () => pr.QueueRemoveAt(Property.Index));
                options.AddOption("");
                options.AddOption("上移至顶", () => {
                    pr.QueueRemoveAt(Property.Index);
                    pr.QueueInsertAt(0, ValueEntry.SmartValue, 0);
                }, Property.Index > 0);
                options.AddOption("上移", () => {
                    pr.QueueRemoveAt(Property.Index);
                    pr.QueueInsertAt(Property.Index - 1, ValueEntry.SmartValue, 0);
                }, Property.Index > 0);
                options.AddOption("下移", () => {
                    pr.QueueRemoveAt(Property.Index);
                    pr.QueueInsertAt(Property.Index + 1, ValueEntry.SmartValue, 0);
                }, Property.Index < pr.MaxCollectionLength - 1);
                options.AddOption("下移至底", () => {
                    pr.QueueRemoveAt(Property.Index);
                    pr.QueueAdd(ValueEntry.SmartValue, 0);
                }, Property.Index < pr.MaxCollectionLength - 1);
                return options.MakeGenericMenu();
            });
        }

        private Rect Draw(GUIContent label, DragHandle dragHandle) {
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(11, false);

            if (Event.current.type == EventType.Repaint) {
                dragHandle.DragHandleRect = rect.AlignLeft(10);
                GUI.Label(rect.AlignLeft(10).AlignMiddle(10), EditorIcons.List.Inactive, GUIStyle.none);
                EditorGUI.DrawRect(rect.AlignLeft(1).AddX(10), GameEditorStyles.BorderColor);
            }

            EditorGUILayout.BeginVertical();
            label.text = "行为";
            CallNextDrawer(label);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            return rect;
        }
    }
}
