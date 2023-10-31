using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace GameEditor.Editors {
    public abstract partial class ItemAssetHandle {
        [Serializable]
        private class GroupAssetHandle<T, U> : BaseAssetHandle<T> where T : GameEditorGroupAsset<U> where U : GameEditorChildAsset {
            public override UnityEngine.Object Selected => m_CurrentAseet;

            //private OdinSelector<T> m_Selector;
            private OdinMenuTree m_MenuTree;

            public GroupAssetHandle(string path) {
                m_AssetPath = $"{path}/{m_AssetPath}/group.asset";
                ReBuild();
            }

            public override void ReBuild() {
                m_MenuTree = new OdinMenuTree();
                m_MenuTree.Config.DrawSearchToolbar = false;
                m_MenuTree.DefaultMenuStyle = GameEditorStyles.OdinListStyle;
                foreach (var group in GameEditorDataUtil.GetAssets<T>(m_AssetPath)) {
                    string gStr = group.ToString();
                    m_MenuTree.Add(gStr, group);
                    string dir = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(group))}/{{0:{typeof(U).GetAttributeCached<NumberRangeAttribute>().Format}}}.asset";
                    foreach (var child in GameEditorDataUtil.GetAssets<U>(dir))
                        m_MenuTree.Add($"{gStr}/{child}", child);
                }
                if (Selected != null)
                    foreach (var gItem in m_MenuTree.EnumerateTree())
                        if (gItem.Value.Equals(Selected))
                            gItem.Select();
            }

            public override void OnBeginDraw(int[] data) {
                float menuWidth = data[2];

                var rect = EditorGUILayout.BeginVertical(GUILayoutOptions.Width(menuWidth).ExpandHeight(true));
                EditorGUI.DrawRect(rect, SirenixGUIStyles.DarkEditorBackground);

                Rect lineRect = rect.AlignRight(1).AddX(1);
                EditorGUI.DrawRect(lineRect, GameEditorStyles.BorderColor);
                menuWidth = Mathf.Clamp(menuWidth + SirenixEditorGUI.SlideRect(lineRect.AlignCenter(7), MouseCursor.ResizeHorizontal).x, 120, 240);

                rect = EditorGUILayout.BeginHorizontal();
                if (Event.current.type == EventType.Repaint)
                    SirenixGUIStyles.ToolbarBackground.Draw(rect, GUIContent.none, 0);
                if (GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("file"), "资产")) {
                    var op = GameEditorGUI.CreateOption();
                    op.AddOption("新建", CreateChild, m_CurrentAseet != null);
                    op.AddOption("复制", Copy, m_CurrentAseet != null && m_CurrentAseet is GameEditorChildAsset);
                    op.AddOption("粘贴", PasteChild, m_CurrentAseet != null && CanPasetChild);
                    op.AddOption("新建组", Create);
                    op.AddOption("删除", Delete, m_CurrentAseet != null && m_CurrentAseet is GameEditorChildAsset);
                    op.AddOption("删除组", Delete, m_CurrentAseet != null && !(m_CurrentAseet is GameEditorChildAsset));
                    op.ShowDropDown(150);
                }
                if (Event.current.type == EventType.Repaint)
                    foreach (var gItem in m_MenuTree.MenuItems) {
                        string gName = gItem.Value.ToString();
                        gItem.Name = gName;
                        foreach (var cItem in gItem.ChildMenuItems)
                            cItem.Name = cItem.Value.ToString();
                    }
                m_MenuTree.DrawSearchToolbar();
                EditorGUILayout.EndHorizontal();
                m_MenuTree.DrawMenuTree();
                EditorGUILayout.EndVertical();
                GUILayoutUtility.GetRect(1, 1, GUILayoutOptions.ExpandWidth(false).ExpandHeight(true));

                if (m_MenuTree.Selection.Count > 0)
                    Select(m_MenuTree.Selection[0].Value as GameEditorAsset);

                data[2] = Mathf.RoundToInt(menuWidth);
            }

            private void CreateChild() {
                var target = m_CurrentAseet is GameEditorChildAsset ? (m_CurrentAseet as GameEditorChildAsset).GetGroup() : m_CurrentAseet;
                var numberConfig = typeof(U).GetAttributeCached<NumberRangeAttribute>();
                var dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(target));
                var path = $"{dir}/{{0:{numberConfig.Format}}}.asset";
                for (int i = numberConfig.Min; i <= numberConfig.Max; i++) {
                    var item = GameEditorDataUtil.CreateAsset<U>(path, i);
                    if (item == null)
                        continue;
                    Select(item);
                    return;
                }
            }

            private void PasteChild() {
                if (!CanPasetChild)
                    return;
                var target = m_CurrentAseet is GameEditorChildAsset ? (m_CurrentAseet as GameEditorChildAsset).GetGroup() : m_CurrentAseet;
                var numberConfig = typeof(U).GetAttributeCached<NumberRangeAttribute>();
                var dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(target));
                var path = $"{dir}/{{0:{numberConfig.Format}}}.asset";
                for (int i = numberConfig.Min; i <= numberConfig.Max; i++) {
                    var item = GameEditorDataUtil.CreateAsset(path, i, s_CopyAsset as U);
                    if (item == null)
                        continue;
                    Select(item);
                    return;
                }
            }

            public bool CanPasetChild => (s_CopyAsset as U) != null;

            public override bool TrySelectMenu(UnityEngine.Object asset) {
                foreach (var item in m_MenuTree.EnumerateTree()) {
                    if ((item.Value as UnityEngine.Object) == asset) {
                        item.Select();
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
