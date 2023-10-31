using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace GameEditor.Editors {
    public abstract partial class ItemAssetHandle {
        [Serializable]
        private class ParentAssetHandle<T> : BaseAssetHandle<T> where T : GameEditorParentAsset {
            public override UnityEngine.Object Selected => m_Recoard.Count == 0 ? null : m_Recoard[m_RecoardIdx];
            [SerializeField]
            private List<UnityEngine.Object> m_Recoard = new List<UnityEngine.Object>();
            [SerializeField]
            private int m_RecoardIdx;
            [SerializeField]
            private UnityEngine.Object m_Selected;
            [SerializeField]
            private string m_MenuSelected;

            private OdinMenuTree m_Menu;

            public ParentAssetHandle() {
                m_AssetPath = $"{m_AssetPath}/data.asset";
                ReBuild();
            }

            private UnityEngine.Object GetMenuSelected() {
                if (!m_CurrentAseet)
                    return null;
                if (m_Menu == null || m_Menu.Selection.Count == 0)
                    return null;
                var menuItem = m_Menu.Selection[0].Value;
                if (menuItem is ItemAssetHandle)
                    return (menuItem as ItemAssetHandle).Selected;
                return menuItem as GameEditorAsset;
            }

            private void UpdateRecoard(UnityEngine.Object asset = null) {
                m_Recoard.RemoveAll(m => !m);
                if (asset == null) {
                    asset = GetMenuSelected();
                    if (asset == null)
                        return;
                    if (asset == m_Selected)
                        return;
                    m_Selected = asset;
                }
                if (m_Recoard.Contains(asset))
                    m_Recoard.Remove(asset);
                m_Recoard.Insert(0, asset);
                m_RecoardIdx = 0;
                if (m_Recoard.Count > 5)
                    m_Recoard.RemoveAt(5);
            }

            public override void OnBeginDraw(int[] data) {
                if (m_CurrentAseet != null)
                    GameEditorError.PushKey(m_CurrentAseet.ToString());
                bool showMenu = data[0] == 0;
                float menuWidth = data[1];

                EditorGUILayout.BeginHorizontal();
                if (SirenixEditorGUI.BeginFadeGroup(this, showMenu)) {
                    EditorGUILayout.BeginVertical(GUILayoutOptions.Width(menuWidth).ExpandHeight(true));
                    Rect rect = GUIHelper.GetCurrentLayoutRect();

                    Rect lineRect = rect.AlignRight(1).AddX(1);
                    EditorGUI.DrawRect(lineRect, GameEditorStyles.BorderColor);
                    menuWidth = Mathf.Clamp(menuWidth + SirenixEditorGUI.SlideRect(lineRect.AlignCenter(7), MouseCursor.ResizeHorizontal).x, 80, 160);

                    EditorGUI.DrawRect(rect, SirenixGUIStyles.MenuBackgroundColor);

                    GameEditorGUI.BeginHorizontalToolbar();
                    if (GameEditorGUI.ToolbarButton(GUIHelper.TempContent("File", GameEditorGUI.GetIcon("file")))) {
                        var op = GameEditorGUI.CreateOption();
                        op.AddOption("New", Create);
                        op.AddOption("Open", () => GameEditorGUI.ShowSelection("Select Asset", GameEditorDataUtil.GetAssets<T>(m_AssetPath), m => Select(m)));
                        op.AddOption("ExportAll", ExportAll);
                        if (m_CurrentAseet != null) {
                            op.AddOption("Export", Export);
                            op.AddOption("Delete", Delete);
                        } else {
                            op.AddOption("Export", null);
                            op.AddOption("Delete", null);
                        }
                        op.ShowDropDown(menuWidth);
                    }
                    GameEditorGUI.EndHorizontalToolbar();

                    if (m_CurrentAseet != null) {
                        if (m_Menu == null)
                            ReBuild();
                        m_Menu.DrawMenuTree();
                    }

                    EditorGUILayout.EndVertical();
                    GUILayoutUtility.GetRect(1, 1, GUILayoutOptions.ExpandWidth(false).ExpandHeight(true));

                    ChildHandle?.BeginDraw(data);
                }
                SirenixEditorGUI.EndFadeGroup();

                EditorGUILayout.BeginVertical();
                if (m_CurrentAseet != null) {
                    GameEditorGUI.BeginHorizontalToolbar();
                    if (GameEditorGUI.ToolbarIconButton(showMenu ? EditorIcons.TriangleLeft.Raw : EditorIcons.TriangleRight.Raw, showMenu ? "收起" : "展开"))
                        showMenu = !showMenu;
                    for (int i = 0; i < m_Recoard.Count; i++) {
                        if (!m_Recoard[i]) {
                            m_Recoard.RemoveAt(i--);
                            continue;
                        }
                        var title = m_Recoard[i].ToString();
                        Texture img = EditorIcons.Transparent.Raw;
                        if (m_Recoard[i] is GameEditorAsset) {
                            var asset = m_Recoard[i] as GameEditorAsset;
                            title = asset.Title.text;
                            img = asset.Title.image;
                        } else {
                        }
                        if (m_RecoardIdx == i) {
                            var rect1 = EditorGUILayout.GetControlRect(false, 1, GameEditorStyles.ToolbarLabel, GUILayoutOptions.Height(21));
                            if (Event.current.type == EventType.Repaint)
                                SirenixGUIStyles.ToolbarButton.Draw(rect1.AddXMax(44), true, true, true, true);
                            GUI.Label(rect1, GUIHelper.TempContent(title), GameEditorStyles.ToolbarLabel);
                            var rect = GUILayoutUtility.GetLastRect().AlignMiddle(14);
                            GUI.DrawTexture(rect.AlignLeft(14).AddX(4), img);
                            if (GameEditorGUI.InlineButton(GameEditorGUI.GetIcon("jump"), "Goto"))
                                TrySelectMenu(null);
                            if (GameEditorGUI.InlineButton(EditorIcons.X.Raw, "Close")) {
                                m_Recoard.RemoveAt(i--);
                                if (m_RecoardIdx == m_Recoard.Count)
                                    m_RecoardIdx = m_Recoard.Count - 1;
                            }
                        } else {
                            if (GUILayout.Toggle(i == m_RecoardIdx, GUIHelper.TempContent(title), GameEditorStyles.ToolbarButton, GUILayoutOptions.Height(21)))
                                m_RecoardIdx = i;
                            var rect = GUILayoutUtility.GetLastRect().AlignMiddle(14);
                            GUI.DrawTexture(rect.AlignLeft(14).AddX(4), img);
                        }
                    }
                    GameEditorGUI.EndHorizontalToolbar();
                }
                data[0] = showMenu ? 0 : 1;
                data[1] = Mathf.RoundToInt(menuWidth);
                if (Event.current.type == EventType.Repaint)
                    UpdateRecoard();
            }

            public override void OnEndDraw() {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                if (m_CurrentAseet != null)
                    GameEditorError.PopKey();
            }

            public override void ReBuild() {
                if (m_Menu != null)
                    m_Menu.MenuItems.ForEach(m => (m.Value as ItemAssetHandle)?.Destroy());
                m_Menu = null;
                if (m_CurrentAseet == null)
                    return;
                m_Menu = new OdinMenuTree();
                m_Menu.DefaultMenuStyle = GameEditorStyles.OdinMenuStyle;
                m_Menu.Selection.SelectionChanged += Selection_SelectionChanged;
                Type currentType = typeof(T);
                var mConfig = currentType.GetAttributeCached<AssetConfigAttribute>();
                m_Menu.Add(mConfig.Name, m_CurrentAseet, GameEditorGUI.GetIcon(mConfig.Icon));
                foreach (var att in currentType.GetAttributes<HasAssetAttribute>()) {
                    var type = att.AssetType;
                    var config = type.GetAttributeCached<AssetConfigAttribute>();
                    m_Menu.Add(config.Name, CreateHandle(type, m_CurrentAseet), GameEditorGUI.GetIcon(config.Icon));
                }
                foreach (OdinMenuItem item in m_Menu.EnumerateTree())
                    if (m_MenuSelected == item.GetFullPath())
                        item.Select(addToSelection: true);
            }

            private void Selection_SelectionChanged(SelectionChangedType type) {
                if (type == SelectionChangedType.ItemAdded)
                    m_MenuSelected = m_Menu.Selection[0].GetFullPath();
            }

            public override void OnSelected() {
                ReBuild();
                m_Recoard.Clear();
                m_Menu.MenuItems[0].Select();
            }

            private ItemAssetHandle ChildHandle => m_Menu != null && m_Menu.Selection.Count > 0 ? m_Menu.Selection[0].Value as ItemAssetHandle : null;

            public override void WeekSelect(UnityEngine.Object asset) {
                base.WeekSelect(asset);
                UpdateRecoard(asset);
            }

            public override bool TrySelectMenu(UnityEngine.Object asset) {
                if (asset != null && asset != m_Recoard[m_RecoardIdx])
                    WeekSelect(asset);
                asset = m_Recoard[m_RecoardIdx];
                m_Selected = asset;
                foreach (var item in m_Menu.MenuItems) {
                    var data = item.Value as GameEditorAsset;
                    if (data == asset) {
                        item.Select();
                        return true;
                    }
                    var handle = item.Value as ItemAssetHandle;
                    if (handle != null && handle.TrySelectMenu(asset)) {
                        item.Select();
                        return true;
                    }
                }
                return false;
            }

            private void Export() {
                Export(m_CurrentAseet as T);
            }

            private void Export(T asset) {
                try {
                    asset.Export();
                } catch (Exception e) {
                    typeof(Exception).GetField("_message", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(e, GameEditorError.Exception(e.Message, out var target, true));
                    if (target != null)
                        TrySelectMenu(target);
                    throw new Exception("Pack Exception", e);
                }
            }

            private void ExportAll() {
                GameEditorDataUtil.GetAssets<T>(m_AssetPath).ForEach(Export);
            }
        }
    }
}