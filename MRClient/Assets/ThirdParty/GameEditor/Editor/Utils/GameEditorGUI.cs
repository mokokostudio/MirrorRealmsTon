using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public static class GameEditorGUI {
        private static GUIStyle s_ToolbarIconBtnStyle;
        private static GUILayoutOption[] s_ToolbarIconBtnOption;

        private static GUIStyle s_ToolbarSelectionStyle;
        private static GUILayoutOption[] s_ToolbarSelectionOption;

        static GameEditorGUI() {
            s_ToolbarIconBtnStyle = new GUIStyle(GUI.skin.FindStyle("RL FooterButton"));
            s_ToolbarIconBtnStyle.padding = new RectOffset(2, 2, 2, 2);
            s_ToolbarIconBtnStyle.overflow = new RectOffset(0, 0, 0, 0);
            s_ToolbarIconBtnStyle.margin = new RectOffset(0, 0, 0, 0);
            s_ToolbarIconBtnStyle.fixedHeight = 21;
            s_ToolbarIconBtnOption = GUILayoutOptions.Width(21).Height(21).ExpandHeight(false).ExpandWidth(false);


            s_ToolbarSelectionStyle = new GUIStyle(SirenixGUIStyles.ToolbarButton);
            s_ToolbarSelectionStyle.padding = new RectOffset(4, 17, 4, 4);
            s_ToolbarSelectionOption = GUILayoutOptions.ExpandWidth(true).Height(21);
        }

        public static void BeginHorizontalToolbar() {
            EditorGUILayout.BeginHorizontal(GUILayoutOptions.Height(21).ExpandWidth(true));
        }
        public static void EndHorizontalToolbar() {
            EditorGUILayout.EndHorizontal();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true)), GameEditorStyles.BorderColor);
        }
        public static bool ToolbarIconButton(Texture texture, string toolTip = "") => GUILayout.Button(new GUIContent(texture, toolTip), s_ToolbarIconBtnStyle, s_ToolbarIconBtnOption);
        public static bool ToolbarButton(GUIContent content) => GUILayout.Button(content, s_ToolbarIconBtnStyle);
        public static bool InlineButton(Texture texture, string toolTip = "") => GUILayout.Button(new GUIContent(texture, toolTip), GameEditorStyles.InlineButton, GUILayoutOptions.Width(18f));
        public static T ToolBarSelection<T>(T current, List<T> list, bool hasNull = false, Func<T, string> nameCall = null) {
            GUIContent label;
            if (!list.Contains(current)) {
                current = default;
                label = new GUIContent("无");
            } else
                label = new GUIContent(nameCall == null ? current.ToString() : nameCall(current));
            var rect = EditorGUILayout.GetControlRect(false, 21, s_ToolbarSelectionStyle, s_ToolbarSelectionOption);
            var selection = GenericSelector<T>.DrawSelectorDropdown(rect, label, m => {
                var selector = new GenericSelector<T>();
                if (hasNull)
                    selector.SelectionTree.Add("无", null);
                selector.SelectionTree.AddRange(list, m => nameCall == null ? m.ToString() : nameCall(m));
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                selector.EnableSingleClickToSelect();
                selector.ShowInPopup(m.AddXMin(-1));
                return selector;
            }, s_ToolbarSelectionStyle);
            GUI.DrawTexture(rect.AlignMiddle(13).AlignRight(13).AddX(-4), EditorIcons.TriangleDown.Raw);
            if (selection == null)
                return current;
            else
                return selection.FirstOrDefault();
        }


        public static object ToolBarTypeSelection<T>(Rect rect, object current) {
            List<Type> types = null;
            var oType = current == null ? null : current.GetType();
            var tType = ToolBarSelection(oType, types);
            if (oType == tType)
                return current;
            if (oType == null)
                return null;
            return Activator.CreateInstance(tType);
        }

        public static T AssetSelection<T>(GUIContent content, T current, List<T> list, bool hasNull = false, bool hasEditor = true, string hasName = "无") where T : GameEditorAsset {
            EditorGUILayout.BeginHorizontal();
            var result = DrawSelection(content, current, list, hasNull, hasName);
            bool enable = GUI.enabled;
            if (result == null)
                GUI.enabled = false;
            if (hasEditor && InlineButton(GetIcon("edit"), "编辑"))
                GameEditorWindow.Current?.Select(result);
            GUI.enabled = enable;
            EditorGUILayout.EndHorizontal();
            return result;
        }

        public static object AssetSelection(GUIContent content, object current, IList list, bool hasNull = false, bool hasEditor = true, string hasName = "无") {
            EditorGUILayout.BeginHorizontal();
            var result = DrawSelection(content, current, list, hasNull, hasName);
            bool enable = GUI.enabled;
            if (result == null)
                GUI.enabled = false;
            if (hasEditor && InlineButton(GetIcon("edit"), "编辑"))
                GameEditorWindow.Current?.Select(result as GameEditorAsset);
            GUI.enabled = enable;
            EditorGUILayout.EndHorizontal();
            return result;
        }

        public static T DrawSelection<T>(GUIContent content, T current, List<T> list, bool hasNull = false, string hasName = "无", Func<T, string> nameCall = null) {
            GUIContent label;
            if (!list.Contains(current)) {
                current = default;
                label = new GUIContent(hasName);
            } else
                label = new GUIContent(nameCall == null ? current.ToString() : nameCall(current));
            var selection = GenericSelector<T>.DrawSelectorDropdown(content, label, m => {
                var selector = new GenericSelector<T>();
                if (hasNull)
                    selector.SelectionTree.Add(hasName, null);
                selector.SelectionTree.AddRange(list, m => nameCall == null ? m.ToString() : nameCall(m));
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                selector.EnableSingleClickToSelect();
                selector.ShowInPopup(m.AddXMin(-1));
                return selector;
            });
            if (selection == null)
                return current;
            else
                return selection.FirstOrDefault();
        }

        public static object DrawSelection(GUIContent content, object current, IList list, bool hasNull = false, string hasName = "无", Func<object, string> nameCall = null) {
            GUIContent label;
            if (!list.Contains(current)) {
                current = default;
                label = new GUIContent(hasName);
            } else
                label = new GUIContent(nameCall == null ? current.ToString() : nameCall(current));
            var selection = GenericSelector<object>.DrawSelectorDropdown(content, label, m => {
                var selector = new GenericSelector<object>();
                if (hasNull)
                    selector.SelectionTree.Add(hasName, null);
                foreach (var obj in list)
                    selector.SelectionTree.Add(nameCall == null ? obj.ToString() : nameCall(obj), obj);
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                selector.EnableSingleClickToSelect();
                selector.ShowInPopup(m.AddXMin(-1));
                return selector;
            });
            if (selection == null)
                return current;
            else
                return selection.FirstOrDefault();
        }

        public static void ShowConfirm(string title, Action call = null, float width = 300) {
            OdinEditorWindow window = null;
            window = OdinEditorWindow.InspectObjectInDropDown(new ConfirmBox(title, () => {
                call?.Invoke();
                window.Close();
            }), width);
        }

        public static void ShowError(string title, Action call = null, float width = 300) {
            OdinEditorWindow window = null;
            window = OdinEditorWindow.InspectObjectInDropDown(new ErrorBox(title, () => {
                call?.Invoke();
                window.Close();
            }), width);
        }

        public static void ShowInputString(string title, Action<string> call, string defaultValue = "", float width = 300) {
            OdinEditorWindow window = null;
            window = OdinEditorWindow.InspectObjectInDropDown(new InputStringBox(title, str => {
                call.Invoke(str);
                window.Close();
            }, defaultValue), width);
        }

        public static Options CreateOption() => new Options();

        public static Texture GetIcon(string name) => Resources.Load<Texture>($"GameIcons/{name}");


        public static void BeginBox() {
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(1, false);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1, false);

            EditorGUI.DrawRect(rect.AlignLeft(1), GameEditorStyles.BorderColor);
            EditorGUI.DrawRect(rect.AlignRight(1), GameEditorStyles.BorderColor);
            rect = rect.AddXMin(1).AddXMax(-1);
            EditorGUI.DrawRect(rect.AlignTop(1), GameEditorStyles.BorderColor);
            EditorGUI.DrawRect(rect.AlignBottom(1), GameEditorStyles.BorderColor);
        }

        public static void EndBox() {
            EditorGUILayout.Space(1, false);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(1, false);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawLine() => EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true)), GameEditorStyles.BorderColor);
        public static void DrawLine(Color color) => EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true)), color);

        public static void AddRightClickArea(Rect rect, Func<GenericMenu> menuCall) {
            GUIHelper.PushGUIEnabled(enabled: true);
            int id = GUIUtility.GetControlID(FocusType.Passive);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition)) {
                GUIUtility.hotControl = id;
                Event.current.Use();
                GUIHelper.RequestRepaint();
            }

            if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition) && id == GUIUtility.hotControl) {
                GUIHelper.RemoveFocusControl();
                Event.current.Use();
                GUIHelper.RemoveFocusControl();
                var genericMenu = menuCall();
                if (genericMenu.GetItemCount() > 0)
                    genericMenu.ShowAsContext();
            }

            if (GUIUtility.hotControl == id && Event.current.type == EventType.Repaint) {
                rect.width = 3f;
                rect.x -= 4f;
                SirenixEditorGUI.DrawSolidRect(rect, SirenixGUIStyles.HighlightedTextColor);
            }
            GUIHelper.PopGUIEnabled();
        }

        public static string Dye(this Color color, string content) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{content}</color>";

        public static void ShowSelection<T>(string title, List<T> list, Action<T> onSelect) => new SelectionDialog<T>(title, list, onSelect).Show();

        public static int ToggleButton(GUIContent label, IEnumerable<string> options, int index) {
            GUILayout.BeginHorizontal();
            SirenixEditorGUI.GetFeatureRichControlRect(label, out int _, out bool _, out Rect rect);
            var len = options.Count();
            var er = options.GetEnumerator();
            rect.width = rect.width / len;
            for (int i = 0; i < len; i++) {
                bool selected = index == i;
                GUIStyle style;
                if (len == 1)
                    style = SirenixGUIStyles.MiniButtonSelected;
                else if (i == 0)
                    style = selected ? SirenixGUIStyles.MiniButtonLeftSelected : SirenixGUIStyles.MiniButtonLeft;
                else if (i == len - 1)
                    style = selected ? SirenixGUIStyles.MiniButtonRightSelected : SirenixGUIStyles.MiniButtonRight;
                else
                    style = selected ? SirenixGUIStyles.MiniButtonMidSelected : SirenixGUIStyles.MiniButtonMid;
                GUIHelper.PushColor((selected ? GameEditorStyles.ActiveColor : GameEditorStyles.InactiveColor) * GUI.color);
                er.MoveNext();
                if (GUI.Button(rect, er.Current, style)) {
                    GUIHelper.RemoveFocusControl();
                    index = i;
                }
                GUIHelper.PopColor();
                rect.x += rect.width;
            }
            GUILayout.EndHorizontal();
            return index;
        }


        private class ConfirmBox {
            private string m_Title;
            private Action m_Action;

            public ConfirmBox(string title, Action call) {
                m_Title = title;
                m_Action = call;
            }

            [InfoBox("@m_Title", InfoMessageType.Warning)]
            [Button("确定")]
            public void Do() => m_Action?.Invoke();
        }

        private class ErrorBox {
            private string m_Title;
            private Action m_Action;

            public ErrorBox(string title, Action call) {
                m_Title = title;
                m_Action = call;
            }

            [InfoBox("@m_Title", InfoMessageType.Error)]
            [Button("确定")]
            public void Do() => m_Action?.Invoke();
        }

        public class InputStringBox {
            private string m_Title;
            private Action<string> m_Action;

            [InfoBox("@m_Title", InfoMessageType.Error)]
            [LabelText("值")]
            public string value;

            [Button("确定")]
            public void Do() => m_Action?.Invoke(value);
            public InputStringBox(string title, Action<string> call, string defaultValue) {
                m_Title = title;
                value = defaultValue;
            }
        }

        public class Option {
            public string title;
            public Action callback;
            public Option(string title, Action callback) {
                this.title = title;
                this.callback = callback;
            }
        }

        public class Options {
            private OdinEditorWindow m_Window;
            private List<Option> m_List = new List<Option>();
            public Options AddOption(string title, Action callback = null) {
                m_List.Add(new Option(title, callback));
                return this;
            }
            public Options AddOption(string title, Action callback, bool enable = true) => AddOption(title, enable ? callback : null);

            public void ShowDropDown(float width) {
                m_Window = OdinEditorWindow.InspectObjectInDropDown(this, width);
            }

            [OnInspectorGUI]
            public void Draw() {
                for (int i = 0; i < m_List.Count; i++) {
                    bool enable = GUI.enabled;
                    if (m_List[i].callback == null)
                        GUI.enabled = false;
                    if (GUILayout.Button(m_List[i].title)) {
                        m_Window.Close();
                        m_List[i].callback?.Invoke();
                    }
                    GUI.enabled = enable;
                }
            }

            public GenericMenu MakeGenericMenu() {
                GenericMenu menu = new GenericMenu();
                foreach (var op in m_List)
                    if (string.IsNullOrEmpty(op.title))
                        menu.AddSeparator("");
                    else if (op.callback == null)
                        menu.AddDisabledItem(new GUIContent(op.title));
                    else
                        menu.AddItem(new GUIContent(op.title), false, () => op.callback());
                return menu;
            }
        }

        [Serializable]
        public class SelectionDialog<T> {
            [BoxGroup("$m_Title")]
            [ShowInInspector]
            [AssetSelecter(list = "m_List", hasEditor = false)]
            [HideLabel]
            private T m_Asset;

            [BoxGroup("$m_Title")]
            [ShowInInspector]
            [Button("确定")]
            [EnableIf("@m_Asset!=null")]
            private void OK() {
                m_Window.Close();
                m_OnSelect?.Invoke(m_Asset);
            }

            private string m_Title;
            private List<T> m_List;
            private OdinEditorWindow m_Window;
            private Action<T> m_OnSelect;

            public SelectionDialog(string title, List<T> list, Action<T> onSelect) {
                m_Title = title;
                m_List = list;
                m_OnSelect = onSelect;
            }

            public void Show() => m_Window = OdinEditorWindow.InspectObjectInDropDown(this, 300);
        }
    }
}
