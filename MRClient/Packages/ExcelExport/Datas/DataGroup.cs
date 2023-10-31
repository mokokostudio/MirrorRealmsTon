using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace ExcelExport {
    public class DataGroup {
        private DataManager m_Manager;
        private string m_Name;
        private string m_Path;
        public EnumData Enums { get; private set; }
        public StructData Structs { get; private set; }
        public ConstantData Constant { get; private set; }
        public LanguageData Language { get; private set; }
        public CustomTableData this[string name] => m_Tables.Find(m => m.Name == name);
        public IEnumerable<CustomTableData> Tables => m_Tables;
        private List<CustomTableData> m_Tables = new List<CustomTableData>();
        public string Location => Name;
        public DataManager Manager => m_Manager;
        public string Name => m_Name;


        public DataGroup(DataManager manager, string path) {
            m_Manager = manager;
            m_Name = Path.GetFileName(path);
            m_Path = path;
            foreach (var fpath in Directory.GetFiles(m_Path)) {
                string fname = Path.GetFileName(fpath);
                string ext = Path.GetExtension(fname);
                if (fname.StartsWith("~") || ext != ".xlsx")
                    continue;
                string tname = Path.GetFileNameWithoutExtension(fname);
                var sheet = new ExcelPackage(fpath).Workbook.Worksheets[0];

                switch (tname) {
                    case "_Enum":
                        Enums = new EnumData(sheet, this);
                        break;
                    case "_Struct":
                        Structs = new StructData(sheet, this);
                        break;
                    case "_Constant":
                        Constant = new ConstantData(sheet, this);
                        break;
                    case "_Language":
                        Language = new LanguageData(sheet, this);
                        break;
                    case "Enum":
                    case "Struct":
                    case "Constant":
                    case "Language":
                        throw new Exception($"{Name} Unsupported table name:{tname}.");
                    default:
                        m_Tables.Add(new CustomTableData(sheet, this, tname));
                        break;
                }
            }
        }

        public void LoadType() {
            Enums?.LoadType();
            Structs?.LoadType();
            Constant?.LoadType();
            Language?.LoadType();
            foreach (var table in m_Tables)
                table.LoadType();
        }

        public void CheckType() {
            Enums?.CheckType();
            Structs?.CheckType();
            Constant?.CheckType();
            foreach (var table in m_Tables)
                table.CheckType();
        }

        public void LoadData() {
            Constant?.LoadData();
            Language?.LoadData();
            foreach (var table in m_Tables)
                table.LoadData();
        }

        public void CheckData() {
            Structs?.CheckData();
            Constant?.CheckData();
            foreach (var table in m_Tables)
                table.CheckData();
        }

        public void ExportData() {
            string dir = $"{Manager.DataPath}/Groups";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string path = $"{dir}/{Name}.bytes";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate)) {
                using (var bw = new BinaryWriter(fs)) {
                    Constant?.Write(bw);
                    Language?.Write(bw);
                    foreach (var table in m_Tables)
                        table.Write(bw);
                }
            }
        }

        public bool HasType(string typeName) {
            switch (typeName) {
                case "ID":
                case "Num":
                case "Str":
                case "Lan":
                    return true;
                default:
                    if (Enums?[typeName] != null)
                        return true;
                    if (Structs?[typeName] != null)
                        return true;
                    if (this[typeName] != null)
                        return true;
                    return false;
            }
        }
    }
}