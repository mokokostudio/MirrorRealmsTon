using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class CustomTableData : BaseTableData {
        public string Name { get; private set; }
        public override string Location => m_Parent.Location + ">" + Name;
        protected override int StartIndex => 1;
        public CustomTableData(ExcelWorksheet sheet, DataGroup group, string name) : base(sheet, group) {
            Name = name;
        }

        public bool IsKeyTable => m_Keys.Count == 1 && m_Keys[0].Type == "ID";

        private List<Field> m_Keys = new List<Field>();
        public List<Field> Keys => m_Keys;

        public override string LanguageKey => m_LanKeyStr;
        private string m_LanKeyStr;

        public override void LoadType() {
            for (int c = StartIndex; c <= m_Sheet.Dimension.End.Column; c++) {
                var range = m_Sheet.Cells[3, c].Text;
                if (!Manager.CheckRange(range))
                    continue;
                var fnc = m_Sheet.Cells[1, c];
                var ftc = m_Sheet.Cells[2, c];
                var field = new Field(this, fnc.Address, fnc.Text, ftc.Text, c);
                AddField(field);
                if (range == "Key") {
                    if (ftc.Text != "ID" && ftc.Text != "Num")
                        throw new Exception($"{Location}>{fnc.Address} Type {ftc.Text} can not be Key.");
                    m_Keys.Add(field);
                }
            }
        }

        public override void LoadData() {
            for (int r = 5; r <= m_Sheet.Dimension.End.Row; r++) {
                if (m_Sheet.Cells[r, 1].Text == "")
                    continue;
                m_LanKeyStr = $"{Group.Name}.{Name}";
                for (int i = 0; i < m_Keys.Count; i++) {
                    var idx = m_Keys[i].SourceIdx;
                    var vc = m_Sheet.Cells[r, idx];
                    ReadField(idx, vc.Address, vc.Text);
                    m_LanKeyStr += "." + vc.Text;
                }
                for (int i = 0; i < m_Fields.Count; i++) {
                    var field = m_Fields[i];
                    if (m_Keys.Contains(field))
                        continue;
                    foreach (var child in field.Childs) {
                        var idx = child.SourceIdx;
                        var vc = m_Sheet.Cells[r, idx];
                        ReadField(idx, vc.Address, vc.Text);
                    }
                }
            }
        }

        public bool CheckKey(ulong key) {
            if (!IsKeyTable) {
                Manager.Log($"{Location} is Not KeyTable.");
                return false;
            }
            return ContainsKey(m_Keys[0].SourceIdx, key);
        }

        public override void Write(BinaryWriter bw) {
            int num = m_Data[StartIndex].Count;
            bw.Write((ushort)num);
            for (int i = 0; i < num; i++)
                foreach (var field in Fields)
                    field.Write(bw, i);
        }
    }
}