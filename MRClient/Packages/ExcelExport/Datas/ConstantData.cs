using OfficeOpenXml;
using System.IO;

namespace ExcelExport {
    public class ConstantData : BaseTableData {
        public override string Location => m_Parent.Location + ">Constant";
        protected override int StartIndex => 2;
        public ConstantData(ExcelWorksheet sheet, DataGroup group) : base(sheet, group) { }
        public override void LoadType() {
            for (int r = StartIndex; r <= m_Sheet.Dimension.End.Row; r++) {
                var range = m_Sheet.Cells[r, 3].Text;
                if (!Manager.CheckRange(range))
                    continue;
                var fnc = m_Sheet.Cells[r, 1];
                var ftc = m_Sheet.Cells[r, 2];
                AddField(new Field(this, fnc.Address, fnc.Text, ftc.Text, r));
            }
        }

        public override void LoadData() {
            for (int r = StartIndex; r <= m_Sheet.Dimension.End.Row; r++) {
                var vc = m_Sheet.Cells[r, 4];
                ReadField(r, vc.Address, vc.Text);
            }
        }

        public override string LanguageKey => $"{Group}.Constant";

        public override void Write(BinaryWriter bw) {
            foreach (var field in Fields)
                field.Write(bw, 0);
        }
    }
}