using System;
using System.Collections.Generic;

namespace ExcelExport {
    public class EnumType {
        private EnumData m_Parent;
        private string m_Address;
        public string Name { get; private set; }
        private List<EnumOption> m_Option = new List<EnumOption>();
        public IEnumerable<EnumOption> Options => m_Option;
        public string Location => m_Parent.Location + ">" + m_Address;

        public EnumOption this[string name] => m_Option.Find(m => m.Name == name);
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;

        public EnumType(EnumData enumData, string address, string name) {
            m_Parent = enumData;
            m_Address = address;
            Name = name;
        }

        public void Add(EnumOption o) {
            if (string.IsNullOrEmpty(o.Name))
                throw new Exception(o.Location + "Enum option is empty.");
            if (this[o.Name] != null)
                throw new Exception(o.Location + " Enum option is duplicate with " + this[o.Name].Location);
            o.Value = m_Option.Count;
            m_Option.Add(o);
        }

        public void CheckType() {
            if (Manager.IsBaseType(Name))
                throw new Exception(Location + " Enum is invalid.");
            if (Manager[Name] != null)
                throw new Exception(Location + " Enum is duplicate with group: " + Manager[Name].Location);
            if (Group[Name] != null)
                throw new Exception(Location + " Enum is duplicate with table: " + Group[Name].Location);
        }


        public Func<string, string, object> GetReader() => ReadEnum;

        public object ReadEnum(string address, string value) {
            var option = this[value];
            if (option == null)
                throw new Exception($"{address} Can not convert to {Location}:{Name}.");
            return option.Value;
        }
    }
}