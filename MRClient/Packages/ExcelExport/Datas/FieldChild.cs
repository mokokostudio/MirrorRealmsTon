using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ExcelExport {
    public class FieldChild : IEquatable<FieldChild> {
        private Field m_Parent;
        private string m_Address;
        private int m_SourceIdx;
        public int ArrayIndex { get; private set; }
        public object GetData(int row) => Table.GetData(row, m_SourceIdx);
        public string StructField { get; private set; }
        public int SourceIdx => m_SourceIdx;

        public string Location => m_Parent.Location + ">" + m_Address;
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;
        public BaseTableData Table => m_Parent.Table;

        public FieldChild(Field field, string address, string name, int index) {
            m_Parent = field;
            m_Address = address;
            ArrayIndex = GetIndex(name);
            StructField = GetField(name);
            m_SourceIdx = index;
        }

        private int GetIndex(string name) {
            var a = name.IndexOf("[");
            if (a == -1)
                return -1;
            var b = name.IndexOf("]");
            var sub = name.Substring(a + 1, b - a - 1);
            return int.Parse(sub);
        }

        private string GetField(string name) {
            var a = name.IndexOf("]");
            var b = name.LastIndexOf(".");
            return name.Substring(Math.Max(a, b) + 1);
        }

        public bool Equals(FieldChild other) => ArrayIndex == other.ArrayIndex && StructField == other.StructField;

        public void CheckType() {
            Table.RegistRead(m_SourceIdx, GetReader());
        }

        public Func<string, string, object> GetReader() {
            var group = string.IsNullOrEmpty(m_Parent.TypeGroup) ? Group : Manager[m_Parent.TypeGroup];
            var st = group.Structs?[m_Parent.Type];
            if (st != null)
                return st.GetReader(StructField);
            var et = group.Enums?[m_Parent.Type];
            if (et != null)
                return et.GetReader();
            var tt = group[m_Parent.Type];
            if (tt != null) {
                if (!tt.IsKeyTable)
                    throw new Exception($"{Location} Table:{tt.Location} is not KeyTable");
                return Manager.ReadID;
            }
            if (m_Parent.Type == "Lan")
                return ReadLan;
            return Manager.GetBaseReader(Location, m_Parent.Type);
        }

        private object ReadLan(string address, string value) {
            return Manager.AddLan(address, $"{Table.LanguageKey}.{m_Parent.Name}", value);
        }
    }
}