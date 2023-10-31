using GameEditor.Editors.Domain;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors {
    [DrawerPriority(1, 0, 0)]
    public class HasDomainAssetDrawer<T> : OdinAttributeDrawer<HasDomainAssetAttribute, T> where T : GameEditorAsset {
        private GUIContent[] m_Contents;
        private PropertyTree[] m_Trees;
        private List<DomainAsset> m_List;
        private int m_Index;

        protected override bool CanDrawAttributeValueProperty(InspectorProperty property) {
            return property.Parent == null;
        }
        protected override void Initialize() {
            int len = Attribute.DomainNames.Length;
            m_Contents = new GUIContent[len];
            m_Trees = new PropertyTree[len];
            m_List = ValueEntry.SmartValue.GetDomainAssets(Attribute.DomainNames);
            for (int i = 0; i < len; i++) {
                m_Contents[i] = new GUIContent(Attribute.DomainNames[i]);
                m_Trees[i] = PropertyTree.Create(m_List[i]);
            }
        }
        protected override void DrawPropertyLayout(GUIContent label) {
            CallNextDrawer(label);
            SirenixEditorGUI.BeginBox("节点");
            GUILayout.BeginHorizontal();
            for (int i = 0; i < m_Contents.Length; i++) {
                bool selected = m_Index == i;
                GUIStyle style;
                if (m_Contents.Length == 1)
                    style = SirenixGUIStyles.MiniButtonSelected;
                else if (i == 0)
                    style = selected ? SirenixGUIStyles.MiniButtonLeftSelected : SirenixGUIStyles.MiniButtonLeft;
                else if (i == m_Contents.Length - 1)
                    style = selected ? SirenixGUIStyles.MiniButtonRightSelected : SirenixGUIStyles.MiniButtonRight;
                else
                    style = selected ? SirenixGUIStyles.MiniButtonMidSelected : SirenixGUIStyles.MiniButtonMid;
                GUIHelper.PushColor((selected ? GameEditorStyles.ActiveColor : GameEditorStyles.InactiveColor) * GUI.color);
                m_Contents[i].image = m_List[i].actions.Count > 0 ? GameEditorGUI.GetIcon("edit") : null;
                if (GUILayout.Button(m_Contents[i], style))
                    m_Index = i;
                GUIHelper.PopColor();
            }
            GUILayout.EndHorizontal();
            m_Trees[m_Index].Draw(true);
            SirenixEditorGUI.EndBox();
        }
    }
}
