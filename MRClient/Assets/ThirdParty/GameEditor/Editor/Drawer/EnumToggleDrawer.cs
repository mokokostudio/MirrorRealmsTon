using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public class EnumToggleDrawer<T> : OdinAttributeDrawer<EnumToggleAttribute, T> {

        private string[] m_Names;
        private Array m_Values;
        private int m_Index;

        protected override void Initialize() {
            Type type = typeof(T);
            m_Names = Enum.GetNames(type);
            m_Values = Enum.GetValues(type);
            for (int i = 0; i < m_Names.Length; i++) {
                var attr = type.GetField(m_Names[i]).GetCustomAttribute<LabelTextAttribute>(true);
                if (attr != null)
                    m_Names[i] = attr.Text;
                if (ValueEntry.WeakSmartValue.Equals(m_Values.GetValue(i)))
                    m_Index = i;
            }
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            var index = GameEditorGUI.ToggleButton(label, m_Names, m_Index);
            if (index != m_Index) {
                ValueEntry.WeakSmartValue = m_Values.GetValue(index);
                m_Index = index;
            }
        }
    }
}
