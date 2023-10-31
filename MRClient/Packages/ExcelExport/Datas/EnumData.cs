using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace ExcelExport {
    public class EnumData {
        private ExcelWorksheet m_Sheet;
        private DataGroup m_Parent;
        private List<EnumType> m_Types = new List<EnumType>();
        public IEnumerable<EnumType> Types => m_Types;
        public EnumType this[string name] => m_Types.Find(m => m.Name == name);
        public string Location => m_Parent.Location + ">Enum";
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent;
        public EnumData(ExcelWorksheet sheet, DataGroup group) {
            m_Sheet = sheet;
            m_Parent = group;
        }

        public void LoadType() {
            var curName = "";
            for (int r = 2; r <= m_Sheet.Dimension.End.Row; r++) {
                var nc = m_Sheet.Cells[r, 1];
                var nv = nc.Text;
                if (!string.IsNullOrEmpty(nv)) {
                    if (nv != curName)
                        AddType(new EnumType(this, nc.Address, nv));
                    var oc = m_Sheet.Cells[r, 2];
                    this[nv].Add(new EnumOption(this[nv], oc.Address, oc.Text));
                }
                curName = nv;
            }
        }

        private void AddType(EnumType type) {
            if (this[type.Name] != null)
                throw new Exception(type.Location + " Enum type is duplicate with " + this[type.Name].Location);
            m_Types.Add(type);
        }

        public void CheckType() {
            foreach (var types in m_Types)
                types.CheckType();
        }
    }
}