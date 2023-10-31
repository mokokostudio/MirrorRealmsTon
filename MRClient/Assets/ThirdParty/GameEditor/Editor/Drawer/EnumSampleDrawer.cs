using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public class EnumSampleDrawer<T> : OdinValueDrawer<T> {
        private static Dictionary<T, string> m_EnumNameDic = new Dictionary<T, string>();
        public static void AddEnumName(T value, string name) => m_EnumNameDic[value] = name;
        public static string GetEnumName(T value) {
            if (m_EnumNameDic.TryGetValue(value, out var name))
                return name;
            var en = value.ToString();
            var labelAt = typeof(T).GetField(en, BindingFlags.Public | BindingFlags.Static).GetAttribute<LabelTextAttribute>();
            if (labelAt == null)
                return en;
            return labelAt.Text;
        }

        private GenericSelector<T> m_Selector;

        private List<T> m_Values = new List<T>();
        private Dictionary<T, string> m_NameDic = new Dictionary<T, string>();

        public override bool CanDrawTypeFilter(Type type) {
            return type.IsEnum;
        }

        protected override void Initialize() {
            Type t = typeof(T);
            foreach (T v in Enum.GetValues(t)) {
                m_Values.Add(v);
                m_NameDic.Add(v, GetEnumName(v));
            }
            m_Selector = new GenericSelector<T>(null, m_Values, false, m => m_NameDic[m]);
            m_Selector.EnableSingleClickToSelect();
            m_Selector.SelectionConfirmed += M_Selector_SelectionConfirmed;
        }

        private void M_Selector_SelectionConfirmed(IEnumerable<T> obj) {
            ValueEntry.SmartValue = obj.SingleOrDefault();
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            SirenixEditorGUI.GetFeatureRichControlRect(label, out _, out _, out var rect);
            var value = ValueEntry.SmartValue;
            var currentName = m_NameDic.ContainsKey(value) ? m_NameDic[value] : "无效";
            if (EditorGUI.DropdownButton(rect, GUIHelper.TempContent(currentName), FocusType.Passive, SirenixGUIStyles.DropDownMiniButton))
                m_Selector.ShowInPopup();
        }
    }
}