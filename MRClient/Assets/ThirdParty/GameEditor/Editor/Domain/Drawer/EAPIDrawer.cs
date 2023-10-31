using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [DrawerPriority(0, 1, 0)]
    public class EAPIDrawer<T> : OdinValueDrawer<T> where T : EAPI {
        private IList m_ParentList;

        protected override void Initialize() {
            m_ParentList = Property.Parent.ValueEntry.WeakSmartValue as IList;
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            T target = ValueEntry.SmartValue;
            if (target != null && !DomainUtil.GetAPITypes(ValueEntry.BaseValueType).Contains(target.GetType())) {
                target = ValueEntry.SmartValue = default;
                Property.SetDirty();
            }
            bool expand = Property.State.Expanded;
            //string text;
            //if (target == null)
            //    text = label?.text + " " + GameEditorStyles.ErrorColor.Dye("未配置");
            //else if (!expand)
            //    text = label?.text + " " + GameEditorStyles.APINameColor.Dye(target.ToString());
            //else
            //    text = label?.text + " " + GameEditorStyles.APINameColor.Dye(typeof(T).GetAPIName());

            //var rect = EditorGUILayout.BeginHorizontal(GameEditorStyles.BoxButton);
            //EditorGUI.DrawRect(rect, expand ? GameEditorStyles.TilteBGColorSelected : GameEditorStyles.TilteBGColor);
            //if (target == null) {
            //    if (GUILayout.Button(GUIHelper.TempContent(text), GameEditorStyles.Label) && Event.current.button == 0) {
            //        DomainUtil.ShowTypeSelection(ValueEntry.BaseValueType, rect, ValueEntry.SmartValue, obj => {
            //            ValueEntry.WeakSmartValue = obj;
            //            Property.SetDirty();
            //        });
            //    }
            //} else
            //    Property.State.Expanded = SirenixEditorGUI.Foldout(expand, GUIHelper.TempContent(text), GameEditorStyles.Foldout);
            //if (target != null && GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("edit"))) {
            //    DomainUtil.ShowTypeSelection(ValueEntry.BaseValueType, rect, ValueEntry.SmartValue, obj => {
            //        ValueEntry.WeakSmartValue = obj;
            //        Property.SetDirty();
            //    });
            //}
            //if (target == null && m_ParentList != null && GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("delete"))) {
            //    m_ParentList.Remove(ValueEntry.WeakSmartValue);
            //    Property.SetDirty();
            //}
            //EditorGUILayout.EndHorizontal();

            var prefixText = label?.text;
            var desText = " " + (target == null ? GameEditorStyles.ErrorColor.Dye("未配置") : expand ? GameEditorStyles.APINameColor.Dye(typeof(T).GetAPIName()) : GameEditorStyles.APINameColor.Dye(target.ToString()));
            if (!expand || target == null)
                prefixText += desText;

            var br = EditorGUILayout.BeginHorizontal(GameEditorStyles.BoxButton);
            if (target == null) {
                if (GUILayout.Button(GUIHelper.TempContent(prefixText), GameEditorStyles.Label) && Event.current.button == 0) {
                    DomainUtil.ShowTypeSelection(ValueEntry.BaseValueType, br, ValueEntry.SmartValue, obj => {
                        ValueEntry.WeakSmartValue = obj;
                        Property.SetDirty();
                    });
                }
            } else {
                if (expand)
                    EditorGUILayout.BeginHorizontal(GUILayoutOptions.Width(GameEditorStyles.Foldout.CalcSize(GUIHelper.TempContent(prefixText)).x));
                Property.State.Expanded = SirenixEditorGUI.Foldout(Property.State.Expanded, GUIHelper.TempContent(prefixText), GameEditorStyles.Foldout);
                if (expand) {
                    EditorGUILayout.EndHorizontal();
                    if (EditorGUILayout.DropdownButton(GUIHelper.TempContent(desText), FocusType.Passive, GameEditorStyles.DropDownMiniButton)) {
                        DomainUtil.ShowTypeSelection(ValueEntry.BaseValueType, br, ValueEntry.SmartValue, obj => {
                            ValueEntry.WeakSmartValue = obj;
                            Property.SetDirty();
                        });
                    }
                }
            }
            if (target == null && m_ParentList != null && GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("delete"))) {
                m_ParentList.Remove(ValueEntry.WeakSmartValue);
                Property.SetDirty();
            }
            EditorGUILayout.EndHorizontal();


            if (target != null) {
                if (SirenixEditorGUI.BeginFadeGroup(this, expand)) {
                    var rect2 = EditorGUILayout.BeginVertical(GUIStyle.none);
                    EditorGUI.DrawRect(rect2, GameEditorStyles.BoxContentColor);
                    CallNextDrawer(label);
                    GUILayout.EndVertical();
                }
                SirenixEditorGUI.EndFadeGroup();
            }
        }
    }

    [DrawerPriority(0, .5f, 0)]
    public class EAPIDrawer2<T> : OdinValueDrawer<T> where T : EAPI {
        protected override void DrawPropertyLayout(GUIContent label) {
            var type = ValueEntry.TypeOfValue;
            bool skip = false;
            if (type.IsGenericType) {
                Type dType = type.GetGenericTypeDefinition();
                if (dType == typeof(ESetVariable<>) || dType == typeof(EArrayAdd<>) || dType == typeof(EArrayRemove<>) || dType == typeof(EGetArrayLength<>)) {
                    GameEditorGUI.DrawLine();
                    var vEntry = Property.Children[0].ValueEntry;
                    Type vDType = vEntry.BaseValueType.GetGenericTypeDefinition();
                    var obj = vEntry.WeakSmartValue;
                    var cr = EditorGUILayout.GetControlRect(true);
                    var ltAttr = Property.Children[0].GetAttribute<LabelTextAttribute>();
                    cr = EditorGUI.PrefixLabel(cr, ltAttr == null ? Property.Children[0].Label : GUIHelper.TempContent(ltAttr.Text));
                    bool drawChild = obj != null;
                    string name = drawChild ? obj.ToString() : "未配置";
                    if (EditorGUI.DropdownButton(cr, GUIHelper.TempContent(name), FocusType.Keyboard)) {
                        DomainUtil.ShowVariableSelection(cr, m => {
                            var vType = m.GetType().GetGenericArguments()[0];
                            if (drawChild && vType == obj.GetType().GetGenericArguments()[0])
                                vEntry.WeakSmartValue = m;
                            else
                                ValueEntry.WeakSmartValue = Activator.CreateInstance(dType.MakeGenericType(vType), m);
                        }, vDType);
                    }
                    if (drawChild) {
                        for (int i = 1; i < Property.Children.Count; i++) {
                            GameEditorGUI.DrawLine();
                            Property.Children[i].Draw();
                        }
                    }
                    skip = true;
                } else if (dType == typeof(ECreateVariable<>) || dType == typeof(ECreateArray<>)) {
                    GameEditorGUI.DrawLine();
                    string vName = Property.Children[0].ValueEntry.WeakSmartValue.ToString();
                    string result = EditorGUILayout.TextField("名字", vName);
                    bool check = DomainUtil.CheckVariableName(result);
                    if (result != vName && check)
                        Property.Children[0].Children[0].ValueEntry.WeakSmartValue = result;
                    else if (result == vName && !check)
                        Property.Children[0].Children[0].ValueEntry.WeakSmartValue = null;
                    GameEditorGUI.DrawLine();
                    Property.Children[1].Draw();
                    skip = true;
                } else if (dType == typeof(EVariablePicker<>) || dType == typeof(EArrayPicker<>)) {
                    GameEditorGUI.DrawLine();
                    var vEntry = Property.Children[0].ValueEntry;
                    var obj = vEntry.WeakSmartValue;
                    var cType = vEntry.BaseValueType;
                    var cr = EditorGUILayout.GetControlRect(true);
                    cr = EditorGUI.PrefixLabel(cr, GUIHelper.TempContent(DomainUtil.GetAPIName(type)));
                    string name = obj == null ? "未配置" : obj.ToString();
                    if (EditorGUI.DropdownButton(cr, GUIHelper.TempContent(name), FocusType.Keyboard)) {
                        DomainUtil.ShowVariableSelection(cr, m => {
                            vEntry.WeakSmartValue = m;
                        }, cType);
                    }
                    skip = true;
                } else if (dType == typeof(EEquate<>)) {
                    GameEditorGUI.DrawLine();
                    var cr = EditorGUILayout.GetControlRect(true);
                    cr = EditorGUI.PrefixLabel(cr, GUIHelper.TempContent("类型"));
                    var cType = type.GetGenericArguments()[0];
                    if (EditorGUI.DropdownButton(cr, GUIHelper.TempContent(DomainUtil.GetValueTypeName(cType)), FocusType.Keyboard)) {
                        DomainUtil.ShowValueTypeSelection(cr, m => {
                            if (m == cType)
                                return;
                            ValueEntry.WeakSmartValue = Activator.CreateInstance(typeof(EEquate<>).MakeGenericType(m));
                        });
                    }
                    GameEditorGUI.DrawLine();
                    Property.Children[0].Draw();
                    GameEditorGUI.DrawLine();
                    Property.Children[1].Draw();
                    skip = true;
                } else if (dType == typeof(EArrayForeach<>)) {
                    GameEditorGUI.DrawLine();
                    var vEntry = Property.Children[0].ValueEntry;
                    Type vDType = vEntry.BaseValueType.GetGenericTypeDefinition();
                    var obj = vEntry.WeakSmartValue;
                    var cr = EditorGUILayout.GetControlRect(true);
                    var ltAttr = Property.Children[0].GetAttribute<LabelTextAttribute>();
                    cr = EditorGUI.PrefixLabel(cr, ltAttr == null ? Property.Children[0].Label : GUIHelper.TempContent(ltAttr.Text));
                    bool drawChild = obj != null;
                    string name = drawChild ? obj.ToString() : "未配置";
                    if (EditorGUI.DropdownButton(cr, GUIHelper.TempContent(name), FocusType.Keyboard)) {
                        DomainUtil.ShowVariableSelection(cr, m => {
                            var vType = m.GetType().GetGenericArguments()[0];
                            if (drawChild && vType == obj.GetType().GetGenericArguments()[0])
                                vEntry.WeakSmartValue = m;
                            else {
                                ValueEntry.WeakSmartValue = Activator.CreateInstance(dType.MakeGenericType(vType), m);
                            }
                        }, vDType);
                    }
                    if (drawChild) {
                        IVariable var = Property.Children[1].ValueEntry.WeakSmartValue as IVariable;
                        string result = EditorGUILayout.TextField("变量名", var.Name);
                        bool check = DomainUtil.CheckVariableName(result);
                        if (result != var.Name && check)
                            var.Name = result;
                        else if (result == var.Name && !check)
                            var.Name = null;
                        DomainUtil.MaskVariablePush(obj as EAPI);
                        DomainUtil.VariableBegin();
                        DomainUtil.AddVariable(var as EAPI);
                        GameEditorGUI.DrawLine();
                        Property.Children[2].Draw();
                        DomainUtil.VariableEnd();
                        DomainUtil.MaskVariablePop(obj as EAPI);
                    }
                    skip = true;
                }
            }
            if (!skip) {
                bool isLastAPI = true;
                for (int i = 0; i < Property.Children.Count; i++) {
                    var property = Property.Children[i];
                    var cType = property.ValueEntry;
                    bool isAPI = typeof(EAPI).IsAssignableFrom(cType.TypeOfValue) || typeof(IList).IsAssignableFrom(cType.TypeOfValue);
                    if (isAPI || isLastAPI)
                        GameEditorGUI.DrawLine();
                    isLastAPI = isAPI;
                    property.Draw();
                }
            }
        }
    }
}