using NUnit.Framework;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEditor.Editors {
    public class AssetSelecterDrawer : OdinAttributeDrawer<AssetSelecterAttribute> {

        private ValueResolver<IEnumerable> m_ListResolver;
        private bool m_IsList;
        private bool m_IsListElement;
        private object result;

        protected override void Initialize() {
            m_ListResolver = ValueResolver.Get<IEnumerable>(Property, Attribute.list);
            m_IsList = Property.ChildResolver as IOrderedCollectionResolver != null;
            m_IsListElement = Property.Info.GetMemberInfo() == null;
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            if (m_IsList) {
                CollectionDrawerStaticInfo.NextCustomAddFunction = OpenSelector;
                CallNextDrawer(label);
                if (result != null) {
                    (Property.ChildResolver as IOrderedCollectionResolver).QueueAdd(new object[] { result });
                    result = null;
                }
            } else {
                Property.ValueEntry.WeakSmartValue = GameEditorGUI.AssetSelection(label, Property.ValueEntry.WeakSmartValue, GetList(), Attribute.hasNull, Attribute.hasEditor,Attribute.hasName);
            }
        }

        private IList GetList() {
            var result = m_ListResolver.GetValue() as IList;
            if (m_IsList || m_IsListElement) {
                var listProperty = Property.FindParent(m => (m.ChildResolver as IOrderedCollectionResolver) != null, true);
                foreach (var obj in listProperty.ValueEntry.WeakValues.Cast<IEnumerable>().FirstOrDefault())
                    if (obj != null && obj != Property.ValueEntry.WeakSmartValue)
                        result.Remove(obj);
            }
            return result;
        }

        private void OpenSelector() {
            var selector = new GenericSelector<object>();
            if (Attribute.hasNull)
                selector.SelectionTree.Add(Attribute.hasName, null);
            var list = GetList();
            foreach (var obj in list)
                selector.SelectionTree.Add(obj.ToString(), obj);
            selector.SelectionTree.Config.DrawSearchToolbar = true;
            selector.EnableSingleClickToSelect();
            selector.ShowInPopup(new Rect(Event.current.mousePosition, Vector2.zero));
            selector.SelectionConfirmed += m => result = m.SingleOrDefault();
        }
    }
}
