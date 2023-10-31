using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class StructField {
        private StructType m_Parent;
        private string m_Address;
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string TypeGroup { get; private set; }
        public object Default { get; private set; }
        private string m_DefaultStr;
        public string Location => m_Parent.Location + ">" + m_Address;
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;
        public StructField(StructType structType, string address, string name, string type, string @default) {
            m_Parent = structType;
            m_Address = address;
            Name = name;
            if (type.Contains(".")) {
                var sp = type.Split(".");
                TypeGroup = sp[0];
                Type = sp[1];
            } else
                Type = type;
            m_DefaultStr = @default;
        }

        public void CheckType() {
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];
            if (!group.HasType(Type))
                throw new Exception(Location + " Struct field Type is invalid.");
        }

        public Func<string, string, object> GetReader() {
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];
            var et = group.Enums[Type];
            if (et != null)
                return et.GetReader();
            var tt = group[Type];
            if (tt != null) {
                if (!tt.IsKeyTable)
                    throw new Exception($"{tt.Location} is not KeyTable");
                return Manager.ReadID;
            }
            if (Type == "Lan")
                return ReadLan;
            return Manager.GetBaseReader(Location, Type);
        }

        private object ReadLan(string address, string value) {
            return Manager.AddLan(address, $"{Group.Name}.Constant.{m_Parent.Name}", value);
        }

        public void CheckData() {
            switch (Type) {
                case "ID":
                    Default = Manager.ReadID(Location, m_DefaultStr);
                    return;
                case "Num":
                    Default = Manager.ReadNum(Location, m_DefaultStr);
                    return;
                case "Str":
                    Default = Manager.ReadStr(Location, m_DefaultStr);
                    return;
                case "Lan":
                    Default = 0;
                    return;
            }
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];

            var eType = group.Enums[Type];
            if (eType != null) {
                var eOption = eType[m_DefaultStr];
                if (eOption == null)
                    throw new Exception($"{Location} Enum {eType.Name} Default Value Error.");
                Default = eOption.Value;
                return;
            }

            var cType = group[Type];
            if (cType != null) {
                Default = Manager.ReadID(Location, m_DefaultStr);
                if (!cType.CheckKey((ulong)Default))
                    throw new Exception($"{Location} Table {cType.Name} Default Value Error.");
                return;
            }

            throw new Exception($"{Location} Unknow Type.");
        }

        internal void Write(BinaryWriter bw, object value) {
            var obj = value ?? Default;
            switch (Type) {
                case "ID":
                    bw.Write((ulong)obj);
                    return;
                case "Num":
                    bw.Write((int)obj);
                    return;
                case "Str":
                    bw.Write((string)obj);
                    return;
                case "Lan":
                    bw.Write((int)obj);
                    return;
            }
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];

            if (group.Enums[Type] != null) {
                bw.Write((int)obj);
                return;
            }
            if (group[Type] != null) {
                bw.Write((ulong)obj);
                return;
            }
        }
    }
}