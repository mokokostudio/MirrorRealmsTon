using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors {
    public class BooleanToggleDrawer : OdinAttributeDrawer<BooleanToggleAttribute, bool> {
        private string[] m_Names;
        protected override void Initialize() {
            base.Initialize();
            m_Names = new string[2];
            m_Names[0] = Attribute.falseName;
            m_Names[1] = Attribute.trueName;
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            ValueEntry.SmartValue = GameEditorGUI.ToggleButton(label, m_Names, ValueEntry.SmartValue ? 1 : 0) == 1;
        }
    }
}