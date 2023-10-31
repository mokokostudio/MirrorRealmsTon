using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class Field {
        private BaseTableData m_Parent;
        private string m_Address;
        public string Name { get; private set; }
        public bool IsArray { get; private set; }
        public string Type { get; private set; }
        public string TypeGroup { get; private set; }
        private List<FieldChild> m_Childs = new List<FieldChild>();
        public IEnumerable<FieldChild> Childs => m_Childs;
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;
        public BaseTableData Table => m_Parent;
        public string Location => m_Parent.Location + ">" + m_Address;
        public Field(BaseTableData table, string address, string name, string type, int index) {
            m_Parent = table;
            m_Address = address;
            if (type.Contains(".")) {
                var sp = type.Split(".");
                TypeGroup = sp[0];
                Type = sp[1];
            } else
                Type = type;
            IsArray = name.Contains("[");
            var isStruct = name.Contains(".");
            if (!IsArray && !isStruct) {
                Name = name;
            } else {
                Name = name.Substring(0, name.LastIndexOf(IsArray ? "[" : "."));
            }
            m_Childs.Add(new FieldChild(this, address, name, index));
        }

        public void Append(Field field) {
            if (Type != field.Type)
                throw new Exception(field.Location + " Invalid type.");
            if (m_Childs.Contains(field.m_Childs[0]))
                throw new Exception(field.Location + " Header duplication.");
            m_Childs.Add(field.m_Childs[0]);
        }

        public void CheckType() {
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];
            if (!group.HasType(Type))
                throw new Exception(Location + " Struct field Type is invalid.");
            foreach (var child in m_Childs)
                child.CheckType();
        }

        public int SourceIdx => m_Childs[0].SourceIdx;

        public void Write(BinaryWriter bw, int row) {
            if (IsArray) {
                List<int> indices = new List<int>();
                Dictionary<int, List<FieldChild>> fieldDics = new Dictionary<int, List<FieldChild>>();
                foreach (var fc in m_Childs) {
                    var idx = fc.ArrayIndex;
                    if (!indices.Contains(idx)) {
                        indices.Add(idx);
                        fieldDics.Add(idx, new List<FieldChild>());
                    }
                    fieldDics[idx].Add(fc);
                }
                for (int i = 0; i < indices.Count; i++)
                    if (!fieldDics[indices[i]].Exists(m => Table.GetData(row, m.SourceIdx) != null))
                        indices.RemoveAt(i--);
                bw.Write((ushort)indices.Count);
                for (int i = 0; i < indices.Count; i++)
                    WriteChild(bw, row, fieldDics[indices[i]]);
            } else
                WriteChild(bw, row, m_Childs);
        }

        public void CheckData(int row) {
            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];
            var table = group[Type];
            if (table != null) {
                var id = (ulong)Table.GetData(row, SourceIdx);
                if (!table.CheckKey(id))
                    throw new Exception($"{Location} {Name}:{id} in {table.Location} Not Found");
            }
        }

        private void WriteChild(BinaryWriter bw, int row, List<FieldChild> childs) {
            var datas = childs.ConvertAll(m => Table.GetData(row, m.SourceIdx));
            var data = datas[0];
            switch (Type) {
                case "ID":
                    if (data == null)
                        bw.Write(0ul);
                    else
                        bw.Write((ulong)data);
                    return;
                case "Num":
                    if (data == null)
                        bw.Write(0);
                    else
                        bw.Write((int)data);
                    return;
                case "Str":
                    if (data == null)
                        bw.Write("");
                    else
                        bw.Write((string)data);
                    return;
                case "Lan":
                    bw.Write((int)datas[0]);
                    return;
            }

            var group = string.IsNullOrEmpty(TypeGroup) ? Group : Manager[TypeGroup];
            if (group.Enums[Type] != null) {
                if (data == null)
                    bw.Write(0);
                else
                    bw.Write((int)data);
                return;
            }
            if (group[Type] != null) {
                if (data == null)
                    bw.Write(0ul);
                else
                    bw.Write((ulong)data);
                return;
            }
            group.Structs[Type].Write(bw, childs, datas);
        }
    }
}