using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public abstract partial class ItemAssetHandle {
        [Serializable]
        private class ChildAssetHandle<T> : BaseAssetHandle<T> where T : GameEditorChildAsset {
            public override UnityEngine.Object Selected => m_CurrentAseet;

            private OdinMenuTree m_MenuTree;

            public ChildAssetHandle(string path) {
                m_AssetPath = $"{path}/{m_AssetPath}.asset";
                ReBuild();
            }

            public override void ReBuild() {
                m_MenuTree = new OdinMenuTree();
                m_MenuTree.Config.SearchToolbarHeight = 22;
                m_MenuTree.DefaultMenuStyle = GameEditorStyles.OdinListStyle;
                foreach (var asset in GameEditorDataUtil.GetAssets<T>(m_AssetPath))
                    m_MenuTree.Add(asset.ToString(), asset);
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
                    var op = GameEditorGUI.CreateOption().AddOption("新建", Create);
                    op.AddOption("复制", Copy, m_CurrentAseet != null);
                    op.AddOption("粘贴", Paste, CanPaste);
                    op.AddOption("删除", Delete, m_CurrentAseet != null);
                    op.ShowDropDown(150);
                }
                if (Event.current.type == EventType.Repaint)
                    foreach (var item in m_MenuTree.MenuItems)
                        item.Name = item.Value.ToString();
                m_MenuTree.DrawSearchToolbar();
                EditorGUILayout.EndHorizontal();
                m_MenuTree.DrawMenuTree();
                EditorGUILayout.EndVertical();
                GUILayoutUtility.GetRect(1, 1, GUILayoutOptions.ExpandWidth(false).ExpandHeight(true));

                if (m_MenuTree.Selection.Count > 0)
                    Select(m_MenuTree.Selection[0].Value as T);

                data[2] = Mathf.RoundToInt(menuWidth);
            }

            public override bool TrySelectMenu(UnityEngine.Object asset) {
                foreach (var item in m_MenuTree.MenuItems) {
                    var handle = item.Value as UnityEngine.Object;
                    if (handle == asset) {
                        item.Select();
                        return true;
                    }
                }
                return false;
            }
        }
    }

}