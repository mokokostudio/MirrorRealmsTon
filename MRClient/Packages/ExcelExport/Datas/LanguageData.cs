using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ExcelExport {
    public class LanguageData {
        private ExcelWorksheet m_Sheet;
        private DataGroup m_Parent;

        private HashSet<string> m_Keys = new HashSet<string>();
        private List<LanguageField> m_Fields = new List<LanguageField>();
        public string Location => m_Parent.Location + ">Language";
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent;

        public IEnumerable<LanguageField> Fields => m_Fields;

        public LanguageData(ExcelWorksheet sheet, DataGroup group) {
            m_Sheet = sheet;
            m_Parent = group;
        }

        public void LoadType() {
            for (int r = 1; r <= m_Sheet.Dimension.End.Row; r++) {
                var kc = m_Sheet.Cells[r, 1];
                var kv = kc.Text;
                if (string.IsNullOrEmpty(kv))
                    continue;
                if (m_Keys.Contains(kv))
                    throw new Exception($"{Location}>{kc.Address} Language Key Duplicate.");
                var vv = m_Sheet.Cells[r, 2].Text;
                m_Fields.Add(new LanguageField(this, kv, vv));
            }
        }

        public void LoadData() {
            for (int i = 0; i < m_Fields.Count; i++)
                m_Fields[i].LoadData();
        }

        public void Write(BinaryWriter bw) {
            for (int i = 0; i < m_Fields.Count; i++)
                m_Fields[i].Write(bw);
        }
    }

    public class LanguageField {
        private LanguageData m_Parent;
        private string m_Key;
        private string m_Value;
        private int m_Idx;
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent.Group;
        public string Name => m_Key;
        public LanguageField(LanguageData data, string key, string value) {
            m_Parent = data;
            m_Key = key;
            m_Value = value;
        }

        public void LoadData() {
            m_Idx = Manager.AddLan("", $"{Group.Name}.Language.{m_Key}", m_Value);
        }
        public void Write(BinaryWriter bw) {
            bw.Write(m_Idx);
        }
    }
}