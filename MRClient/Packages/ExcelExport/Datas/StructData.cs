using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace ExcelExport {
    public class StructData {
        private ExcelWorksheet m_Sheet;
        private DataGroup m_Parent;
        private List<StructType> m_Types = new List<StructType>();
        public IEnumerable<StructType> Types => m_Types;
        public StructType this[string name] => m_Types.Find(m => m.Name == name);
        public string Location => m_Parent.Location + ">Struct";
        public DataManager Manager => m_Parent.Manager;
        public DataGroup Group => m_Parent;
        public StructData(ExcelWorksheet sheet, DataGroup group) {
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
                        AddType(new StructType(this, nc.Address, nv));
                    var fnc = m_Sheet.Cells[r, 2];
                    var ftc = m_Sheet.Cells[r, 3];
                    var fdc = m_Sheet.Cells[r, 4];
                    this[nv].Add(new StructField(this[nv], fnc.Address, fnc.Text, ftc.Text, fdc.Text));
                }
                curName = nv;
            }
        }

        private void AddType(StructType type) {
            if (this[type.Name] != null)
                throw new Exception(type.Location + " Struct type is duplicate with " + this[type.Name].Location);
            m_Types.Add(type);
        }

        public void CheckType() {
            foreach (var types in m_Types)
                types.CheckType();
        }

        internal void CheckData() {
            foreach (var types in m_Types)
                types.CheckData();
        }
    }
}