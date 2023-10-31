using GameEditor.Domain;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameEditor.Editors.Domain {
    public abstract class ExportFactroy<G, T> where G : DomainGroup<T>, new() where T : RunHandle {
        private Dictionary<object, int> m_VariableDic = new Dictionary<object, int>();
        private HashSet<int> m_VariableKeys = new HashSet<int>();
        private List<List<object>> m_VariableData = new List<List<object>>();
        private List<object> m_LastVariableData => m_VariableData[m_VariableData.Count - 1];

        private void BeginVariableArea() {
            m_VariableData.Add(new List<object>());
        }
        private void EndVariableArea() {
            new List<object>(m_LastVariableData).ForEach(RemoveVariable);
            m_VariableData.RemoveAt(m_VariableData.Count - 1);
        }

        private int AddVariable(object obj) {
            int i = 1;
            while (true) {
                if (m_VariableKeys.Contains(i)) {
                    i++;
                    continue;
                }
                m_VariableKeys.Add(i);
                m_VariableDic.Add(obj, i);
                m_LastVariableData.Add(obj);
                return i;
            }
        }

        private void RemoveVariable(object obj) {
            if (!m_VariableDic.ContainsKey(obj))
                return;
            m_VariableKeys.Remove(m_VariableDic[obj]);
            m_VariableDic.Remove(obj);
            m_LastVariableData.Remove(obj);
        }

        private int VariableID(object obj) => m_VariableDic[obj];

        public G Group;
        public ExportFactroy() {
            Group = new G();
        }
        public APIActRuntime<T> Export(DomainAsset asset) => DoConvert(asset);
        public APIReturnRuntime<T, bool> Export(ConditionBox box) => Convert(box.condition);

        public List<APIActRuntime<T>> Convert(List<EAPIAct> api) {
            BeginVariableArea();
            var result = api.ConvertAll(m => Convert(m));
            EndVariableArea();
            return result;
        }
        public APIActRuntime<T> Convert(EAPIAct api) {
            return ReflectionInvoke<APIActRuntime<T>>(api);
        }

        public APIReturnRuntime<T, U> Convert<U>(EAPIReturn<U> api) {
            return ReflectionInvoke<APIReturnRuntime<T, U>>(api);
        }

        public APIReturnArrayRuntime<T, U> Convert<U>(EAPIReturnArray<U> api) {
            return ReflectionInvoke<APIReturnArrayRuntime<T, U>>(api);
        }

        private U ReflectionInvoke<U>(EAPI api) {
            Type aType = api.GetType();
            MethodInfo method = null;
            foreach (var m in GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
                if (m.Name != "DoConvert")
                    continue;
                Type pType = m.GetParameters()[0].ParameterType;
                if (aType == pType) {
                    method = m;
                    break;
                }
                if (pType.IsGenericType && aType.IsGenericType && pType.GetGenericTypeDefinition() == aType.GetGenericTypeDefinition()) {
                    method = m.MakeGenericMethod(aType.GenericTypeArguments[0]);
                    break;
                }
            }
            if (method == null)
                throw new Exception("未添加的导出类型:" + aType);
            return (U)method.Invoke(this, new object[] { api });
        }

        public virtual APIActRuntime<T> DoConvert(DomainAsset asset) {
            var result = Group.CreateDomain();
            result.list = Convert(asset.actions);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert(EFork api) {
            var result = Group.CreateFork();
            result.ifList = new List<DomainGroup<T>.Fork.IF>();
            foreach (var item in api.elseif) {
                var data = new DomainGroup<T>.Fork.IF();
                data.condition = Convert(item.cond);
                data.acts = Convert(item.actions);
                result.ifList.Add(data);
            }
            result.acts = Convert(api.@else.actions);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(EArrayAdd<U> api) {
            var result = Group.CreateArrayAdd<U>();
            result.id = VariableID(api.array);
            result.value = Convert(api.target);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(EArrayForeach<U> api) {
            AddVariable(api.item);
            var result = Group.CreateArrayForeach<U>();
            result.id = VariableID(api.array);
            result.acts = Convert(api.actions);
            RemoveVariable(api.item);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(EArrayRemove<U> api) {
            var result = Group.CreateArrayRemove<U>();
            result.id = VariableID(api.array);
            result.value = Convert(api.target);
            return result;
        }
        public virtual APIReturnRuntime<T, float> DoConvert<U>(EGetArrayLength<U> api) {
            var result = Group.CreateArrayLength<U>();
            result.id = VariableID(api.array);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(ECreateArray<U> api) {
            AddVariable(api.array);
            var result = Group.CreateCreateArray<U>();
            result.value = Convert(api.value);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(ECreateVariable<U> api) {
            AddVariable(api.variable);
            var result = Group.CreateCreateVariable<U>();
            result.value = Convert(api.value);
            return result;
        }
        public virtual APIActRuntime<T> DoConvert<U>(ESetVariable<U> api) {
            var result = Group.CreateSetVariable<U>();
            result.id = VariableID(api.variable);
            result.value = Convert(api.value);
            return result;
        }

        public virtual APIReturnRuntime<T, U> DoConvert<U>(EStatic<U> api) {
            var result = Group.CreateStatic<U>();
            result.value = api.value;
            return result;
        }
        public virtual APIReturnRuntime<T, U> DoConvert<U>(EVariablePicker<U> api) {
            var result = Group.CreateVariablePicker<U>();
            result.id = VariableID(api.variable);
            return result;
        }

        public virtual APIReturnRuntime<T, bool> DoConvert(EAnd api) {
            var result = Group.CreateAnd();
            result.a = Convert(api.condA);
            result.b = Convert(api.condB);
            return result;
        }
        public virtual APIReturnRuntime<T, bool> DoConvert(ECompareNumber api) {
            var result = Group.CreateCompareNumber();
            result.compareType = api.compareType;
            result.a = Convert(api.numberA);
            result.b = Convert(api.numberB);
            return result;
        }
        public virtual APIReturnRuntime<T, bool> DoConvert(ENO api) {
            var result = Group.CreateNo();
            result.a = Convert(api.cond);
            return result;
        }
        public virtual APIReturnRuntime<T, bool> DoConvert(EOR api) {
            var result = Group.CreateOR();
            result.a = Convert(api.condA);
            result.b = Convert(api.condB);
            return result;
        }
        public virtual APIReturnRuntime<T, bool> DoConvert<U>(EEquate<U> api) {
            var result = Group.CreateEquate<U>();
            result.a = Convert(api.valueA);
            result.b = Convert(api.valueB);
            return result;
        }

        public virtual APIReturnArrayRuntime<T, U> DoConvert<U>(EArrayEmpty<U> api) {
            var result = Group.CreateArrayEmpty<U>();
            return result;
        }
        public virtual APIReturnArrayRuntime<T, U> DoConvert<U>(EArrayPicker<U> api) {
            var result = Group.CreateArrayPicker<U>();
            result.id = VariableID(api.array);
            return result;
        }
    }
}
