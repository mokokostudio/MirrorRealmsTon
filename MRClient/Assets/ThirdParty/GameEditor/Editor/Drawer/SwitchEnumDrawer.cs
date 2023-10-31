using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameEditor.Editors {
    public class SwitchEnumDrawer<T> : OdinValueDrawer<SwitchEnum<T>> where T : Enum {
        private List<EnumTypeUtilities<T>.EnumMember> m_MemberList = new List<EnumTypeUtilities<T>.EnumMember>();
        private EnumTypeUtilities<T>.EnumMember m_ZeroMember;
        protected override void Initialize() {
            m_ZeroMember = new EnumTypeUtilities<T>.EnumMember {
                Value = (T)(object)0,
                Name = "无",
                NiceName = "无",
                IsObsolete = false,
                Message = ""
            };
            foreach (var member in EnumTypeUtilities<T>.AllEnumMemberInfos) {
                if (member.Hide)
                    continue;
                if (IsP2(member.Value))
                    m_MemberList.Add(member);
                else if (IsZero(member.Value))
                    m_ZeroMember = member;
            }
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            SirenixEditorGUI.GetFeatureRichControlRect(label, out int controlId, out bool _, out Rect valueRect);
            var selection = OdinSelector<SwitchEnum<T>>.DrawSelectorDropdown(valueRect, GetDes(ValueEntry.SmartValue), rect => {
                var selector = new SwitchEnumSelector();
                selector.SetValue(ValueEntry.SmartValue);
                selector.ShowInPopup(rect);
                return selector;
            });
            if (selection != null)
                ValueEntry.SmartValue = selection.SingleOrDefault();
        }

        private string GetDes(SwitchEnum<T> value) {
            if (Convert.ToInt32(value.white) == 0)
                return m_ZeroMember.NiceName;
            StringBuilder sb = new StringBuilder();
            int n = 0;
            foreach (var member in m_MemberList)
                if (value.CheckWhite(member.Value)) {
                    if (n++ > 0)
                        sb.Append("|");
                    sb.Append(member.NiceName);
                }
            if (Convert.ToInt32(value.black) == 0)
                return sb.ToString();
            n = 0;
            sb.Append(", 排除");
            foreach (var member in m_MemberList)
                if (value.CheckBlack(member.Value)) {
                    if (n++ > 0)
                        sb.Append("|");
                    sb.Append(member.NiceName);
                }
            return sb.ToString();
        }

        private static bool IsP2(T value) {
            int v = Convert.ToInt32(value);
            return v > 0 && (v & (v - 1)) == 0;
        }
        private static bool IsZero(T value) {
            int v = Convert.ToInt32(value);
            return v == 0;
        }


        private class SwitchEnumSelector : OdinSelector<SwitchEnum<T>> {

            private int m_White;
            private int m_Black;

            public void SetValue(SwitchEnum<T> value) {
                m_White = Convert.ToInt32(value.white);
                m_Black = Convert.ToInt32(value.black);
            }

            protected override void BuildSelectionTree(OdinMenuTree tree) {
                tree.Selection.SupportsMultiSelect = true;
                tree.Config.DrawSearchToolbar = true;
                tree.Config.ConfirmSelectionOnDoubleClick = false;
                tree.DefaultMenuStyle.Offset += 15f;
                foreach (var member in EnumTypeUtilities<T>.AllEnumMemberInfos) {
                    if (member.Hide)
                        continue;
                    if (IsP2(member.Value))
                        tree.Add(member.NiceName, member);
                }
                foreach (var item in tree.EnumerateTree())
                    item.OnDrawItem += DrawEnumItem;
            }

            public override IEnumerable<SwitchEnum<T>> GetCurrentSelection() {
                var result = new SwitchEnum<T>();
                result.white = (T)Enum.ToObject(typeof(T), m_White);
                result.black = (T)Enum.ToObject(typeof(T), m_Black);
                yield return result;
            }

            private void DrawEnumItem(OdinMenuItem item) {
                if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) && item.Rect.Contains(Event.current.mousePosition)) {
                    if (Event.current.type == EventType.MouseDown) {
                        ToggleEnum(item);
                        TriggerSelectionChanged();
                    }
                    Event.current.Use();
                }
                if (Event.current.type != EventType.Repaint)
                    return;
                Rect rect = item.Rect.AlignLeft(30f).AlignCenter(EditorIcons.TestPassed.width, EditorIcons.TestPassed.height);
                if ((m_White & Convert.ToInt32(GetItemEnum(item))) != 0)
                    GUI.DrawTexture(rect, EditorIcons.TestPassed);
                else if ((m_Black & Convert.ToInt32(GetItemEnum(item))) != 0)
                    GUI.DrawTexture(rect, EditorIcons.TestFailed);
                else
                    GUI.DrawTexture(rect, EditorIcons.TestNormal);
            }

            private void ToggleEnum(OdinMenuItem item) {
                var v = Convert.ToInt32(GetItemEnum(item));
                if ((v & m_White) == v) {
                    m_White &= ~v;
                    m_Black |= v;
                } else if ((v & m_Black) == v) {
                    m_Black &= ~v;
                } else {
                    m_White |= v;
                }
            }

            private T GetItemEnum(OdinMenuItem item) {
                var member = (EnumTypeUtilities<T>.EnumMember)item.Value;
                return member.Value;
            }
        }

    }
}
