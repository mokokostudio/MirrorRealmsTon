using System.Collections.Generic;
namespace GameEditor {
    public abstract class RunHandle {
        public Dictionary<int, object> m_VariableDicID = new Dictionary<int, object>();
        public List<List<int>> m_VariableData = new List<List<int>>();
        private List<int> m_LastVariableData => m_VariableData[m_VariableData.Count - 1];
        public bool m_CopyFlag = false;

        public int CreateArray<T>(List<T> data) {
            return AddVariable(data);
        }
        public List<T> GetArray<T>(int id) {
            return (List<T>)m_VariableDicID[id];
        }

        private int AddVariable(object obj) {
            int i = 1;
            while (true) {
                if (m_VariableDicID.ContainsKey(i)) {
                    i++;
                    continue;
                }
                m_VariableDicID.Add(i, obj);
                m_LastVariableData.Add(i);
                return i;
            }
        }

        public int CreateVariable<T>(T value) {
            return AddVariable(value);
        }

        public T GetVariable<T>(int id) {
            return (T)m_VariableDicID[id];
        }

        public void SetVariable<T>(int id, T value) {
            m_VariableDicID[id] = value;
        }

        public void RemoveVarialbe(int id) {
            if (!m_VariableDicID.ContainsKey(id))
                return;
            m_VariableDicID.Remove(id);
            m_LastVariableData.Remove(id);
        }
        public void BeginVariableArea() {
            m_VariableData.Add(new List<int>());
        }
        public void EndVariableArea() {
            if (!m_CopyFlag) {
                new List<int>(m_LastVariableData).ForEach(RemoveVarialbe);
                m_VariableData.RemoveAt(m_VariableData.Count - 1);
            }
        }
    }
}
