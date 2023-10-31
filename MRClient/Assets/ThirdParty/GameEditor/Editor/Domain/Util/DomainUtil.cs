using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    public static class DomainUtil {
        private class TypeDeclare {
            public Type APIType;
            public Type valueType;
            public string typeName;
            public Func<object, string> valueNameCall;
            public Color color;
            public bool canStatic;
        }
        private static List<TypeDeclare> s_TypeDeclares = new List<TypeDeclare>();
        private static List<Assembly> s_Assemblys = new List<Assembly>();
        public static List<EAPIAct> dropToList;

        public static void AddTypeDeclare<T>(string typeName, Func<T, string> valueNameCall, Color color, bool canStatic) {
            Type type = typeof(T);
            TypeDeclare td = new TypeDeclare();
            td.APIType = typeof(EAPIReturn<>).MakeGenericType(type);
            td.valueType = type;
            td.typeName = typeName;
            td.valueNameCall = m => valueNameCall((T)m);
            td.color = color;
            td.canStatic = canStatic;
            s_TypeDeclares.Add(td);
        }

        public static void AddAssembly(Assembly assembly) {
            if (assembly == null)
                return;
            s_Assemblys.Add(assembly);
        }

        static DomainUtil() {
            AddTypeDeclare<float>("数字", m => m.ToString(), new Color(.5f, 1, 0), true);
            AddTypeDeclare<bool>("布尔", m => m ? "真" : "假", new Color(1, 1, 0), true);
            AddTypeDeclare<string>("字符串", m => m, new Color(1, .5f, 0), true);
            AddAssembly(Assembly.Load("Assembly-CSharp-Editor"));
        }

        public static string GetAPIName(this Type type) {
            var attr = type.GetAttribute<APINameAttribute>();
            if (attr == null)
                return GameEditorStyles.ErrorColor.Dye("未命名" + type.Name);
            if (type.IsGenericType) {
                Type gType = type.GetGenericTypeDefinition();
                if (gType == typeof(ECreateVariable<>) || gType == typeof(ECreateArray<>))
                    return attr.name + "/" + s_TypeDeclares.Find(m => m.valueType == type.GetGenericArguments()[0]).typeName;
            }
            return attr.name;
        }

        public static Color GetColor(this Type type) {
            var td = s_TypeDeclares.Find(m => m.valueType == type);
            if (td == null)
                return Color.white;
            return td.color;
        }

        public static void ShowTypeSelection(Type type, Rect rect, object current, Action<object> call) {
            List<Type> types = GetAPITypes(type);
            var oType = current == null ? null : current.GetType();
            var selector = new GenericSelector<Type>();
            selector.SelectionTree.AddRange(types, m => m.GetAPIName());
            selector.SelectionTree.Config.DrawSearchToolbar = true;
            selector.EnableSingleClickToSelect();
            selector.SelectionConfirmed += selection => {
                var tType = selection.FirstOrDefault();
                if (oType == tType)
                    call(current);
                else if (tType != null)
                    call(Activator.CreateInstance(tType));
            };
            selector.ShowInPopup(rect);
        }

        public static void ShowVariableSelection(Rect rect, Action<EAPI> call, Type type = null) {
            var variables = type == null ? GetVariables() : GetVariables(type);
            var selector = new GenericSelector<EAPI>();
            selector.SelectionTree.AddRange(variables, m => m.ToString());
            selector.SelectionTree.Config.DrawSearchToolbar = true;
            selector.EnableSingleClickToSelect();
            selector.SelectionConfirmed += selection => {
                var obj = selection.FirstOrDefault();
                if (obj != null)
                    call(obj);
            };
            selector.ShowInPopup(rect);
        }

        public static void ShowValueTypeSelection(Rect rect, Action<Type> call) {
            var selector = new GenericSelector<Type>();
            selector.SelectionTree.AddRange(s_TypeDeclares.ConvertAll(m => m.valueType), GetValueTypeName);
            selector.SelectionTree.Config.DrawSearchToolbar = true;
            selector.EnableSingleClickToSelect();
            selector.SelectionConfirmed += selection => {
                var obj = selection.FirstOrDefault();
                if (obj != null)
                    call(obj);
            };
            selector.ShowInPopup(rect);
        }

        public static string GetValueTypeName(Type type) {
            var td = s_TypeDeclares.Find(n => n.valueType == type);
            return td == null ? GameEditorStyles.ErrorColor.Dye("无法识别类型") : td.typeName;
        }

        private static Dictionary<Type, List<Type>> s_APITypesCache = new Dictionary<Type, List<Type>>();

        public static List<Type> GetAPITypes(Type parent) {
            if (s_APITypesCache.ContainsKey(parent))
                return s_APITypesCache[parent];
            List<Type> list = new List<Type>();
            FillTypeList(parent, Assembly.GetExecutingAssembly(), list);
            foreach (Assembly assembly in s_Assemblys)
                FillTypeList(parent, assembly, list);
            s_APITypesCache[parent] = list;
            return list;
        }

        private static void FillTypeList(Type parent, Assembly assembly, List<Type> list) {
            var isGeneric = parent.IsGenericType;
            var parentArgs = parent.GetGenericArguments();
            var td = s_TypeDeclares.Find(m => m.APIType == parent);
            foreach (var type in assembly.GetExportedTypes()) {
                if (type.GetAttribute<APINameAttribute>() == null)
                    continue;
                if (td != null && !td.canStatic && type == typeof(EStatic<>))
                    continue;
                if (type.IsGenericType) {
                    if (isGeneric && parent.GetGenericTypeDefinition().IsAssignableFrom(type))
                        list.Add(type.MakeGenericType(parentArgs));
                    else
                        foreach (var td2 in s_TypeDeclares) {
                            var cType = type.MakeGenericType(td2.valueType);
                            if (parent.IsAssignableFrom(cType))
                                list.Add(cType);
                        }
                } else if (parent.IsAssignableFrom(type))
                    list.Add(type);
            }
        }

        public static string ValueToString<T>(T value) {
            Type type = typeof(T);
            var td = s_TypeDeclares.Find(m => m.valueType == type);
            if (td == null)
                return GameEditorStyles.ErrorColor.Dye("未注册的类型");
            return td.valueNameCall(value);
        }

        private static List<List<EAPI>> s_VariableData = new List<List<EAPI>>();
        private static List<EAPI> s_VariableMaskData = new List<EAPI>();

        public static void VariableBegin() => s_VariableData.Add(new List<EAPI>());
        public static void VariableEnd() => s_VariableData.RemoveAt(s_VariableData.Count - 1);
        public static void AddVariable(EAPI variable) => s_VariableData[s_VariableData.Count - 1].Add(variable);
        public static void MaskVariablePush(EAPI variable) => s_VariableMaskData.Add(variable);
        public static void MaskVariablePop(EAPI variable) => s_VariableMaskData.Remove(variable);
        public static List<EAPI> GetVariables() {
            List<EAPI> list = new List<EAPI>();
            foreach (var datas in s_VariableData)
                foreach (var data in datas)
                    if (!string.IsNullOrEmpty(data.ToString()) && !s_VariableMaskData.Contains(data))
                        list.Add(data);
            return list;
        }
        public static List<EAPI> GetVariables(Type type) {
            List<EAPI> list = new List<EAPI>();
            if (type.IsGenericTypeDefinition) {
                foreach (var datas in s_VariableData)
                    foreach (var data in datas)
                        if (!string.IsNullOrEmpty(data.ToString()) && !s_VariableMaskData.Contains(data) && data.GetType().GetGenericTypeDefinition() == type)
                            list.Add(data);
            } else {
                foreach (var datas in s_VariableData)
                    foreach (var data in datas)
                        if (!string.IsNullOrEmpty(data.ToString()) && !s_VariableMaskData.Contains(data) && data.GetType() == type)
                            list.Add(data);
            }
            return list;
        }

        public static bool CheckVariable(EAPI variable) {
            if (variable == null || string.IsNullOrEmpty(variable.ToString()))
                return false;
            foreach (var datas in s_VariableData)
                foreach (var data in datas)
                    if (data == variable && !s_VariableMaskData.Contains(data))
                        return true;
            return false;
        }

        public static bool CheckVariableName(string name) {
            foreach (var datas in s_VariableData)
                foreach (var data in datas)
                    if (data.ToString() == name)
                        return false;
            return true;
        }

        public static string GetAvailableVariableName() {
            for (int i = 65; i < 91; i++) {
                string name = ((char)i).ToString();
                if (CheckVariableName(name))
                    return name;
            }
            return "";
        }

        public static void CheckVariableCreate(IVariable obj) {
            if (obj == null)
                return;
            if (obj.Name == null)
                obj.Name = GetAvailableVariableName();
            else if (!CheckVariableName(obj.Name))
                obj.Name = "";
        }
    }

}
