using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class BaseTableData {
        protected ExcelWorksheet m_Sheet;
        protected DataGroup m_Parent;
        protected List<Field> m_Fields = new List<Field>();
        public IEnumerable<Field> Fields => m_Fields;
        protected virtual int StartIndex { get; }
        public virtual string Location { get; }
        public Field this[string name] => m_Fields.Find(m => m.Name == name);
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent;

        private Dictionary<int, Func<string, string, object>> m_Reader = new Dictionary<int, Func<string, string, object>>();
        protected Dictionary<int, List<object>> m_Data = new Dictionary<int, List<object>>();
        public BaseTableData(ExcelWorksheet sheet, DataGroup group) {
            m_Sheet = sheet;
            m_Parent = group;
        }

        public virtual void LoadType() { }
        public virtual void LoadData() { }

        protected void AddField(Field field) {
            if (this[field.Name] == null)
                m_Fields.Add(field);
            else
                this[field.Name].Append(field);
        }

        public void CheckType() {
            foreach (var field in m_Fields)
                field.CheckType();
        }

        public void RegistRead(int index, Func<string, string, object> reader) {
            m_Reader[index] = reader;
            m_Data[index] = new List<object>();
        }

        public void ReadField(int index, string address, string value) {
            if (m_Reader.ContainsKey(index))
                m_Data[index].Add(m_Reader[index](address, value));
        }

        public bool ContainsKey(int idx, ulong key) => m_Data[idx].Contains(key);

        public virtual string LanguageKey { get; }

        public virtual void Write(BinaryWriter bw) { }

        public object GetData(int row, int sourceIdx) => m_Data[sourceIdx][row];

        public void CheckData() {
            int num = m_Data[StartIndex].Count;
            for (int i = 0; i < num; i++)
                foreach (var field in m_Fields)
                    field.CheckData(i);
        }
    }
}