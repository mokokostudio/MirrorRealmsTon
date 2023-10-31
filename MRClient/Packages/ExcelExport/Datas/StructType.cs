using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class StructType {
        private StructData m_Parent;
        private string m_Address;
        public string Name { get; private set; }
        private List<StructField> m_Fields = new List<StructField>();
        public IEnumerable<StructField> Fields => m_Fields;
        public string Location => m_Parent.Location + ">" + m_Address;
        public StructField this[string name] => m_Fields.Find(m => m.Name == name);
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;
        public StructType(StructData structData, string address, string name) {
            m_Parent = structData;
            m_Address = address;
            Name = name;
        }

        public void Add(StructField o) {
            if (this[o.Name] != null)
                throw new Exception(o.Location + " Struct field is duplicate with: " + this[o.Name].Location);
            m_Fields.Add(o);
        }

        public void CheckType() {
            if (Manager.IsBaseType(Name))
                throw new Exception(Location + " Struct is invalid.");
            if (Manager[Name] != null)
                throw new Exception(Location + " Struct is duplicate with group: " + Manager[Name].Location);
            if (Group[Name] != null)
                throw new Exception(Location + " Struct is duplicate with table: " + Group[Name].Location);
            foreach (var field in m_Fields)
                field.CheckType();
        }


        public Func<string, string, object> GetReader(string fName) {
            var field = this[fName];
            if(field == null)
                throw new Exception($"{fName} Struct field reference is invalid.");
            return field.GetReader();
        }

        public void CheckData() {
            foreach (var field in m_Fields)
                field.CheckData();
        }

        public void Write(BinaryWriter bw, List<FieldChild> childs, List<object> datas) {
            foreach (var field in m_Fields) {
                var index = childs.FindIndex(m => m.StructField == field.Name);
                field.Write(bw, index == -1 ? null : datas[index]);
            }
        }
    }
}