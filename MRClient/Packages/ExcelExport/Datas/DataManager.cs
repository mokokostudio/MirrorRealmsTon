using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelExport {
    public class DataManager {
        private string m_ExcelPath;
        public string CodePath { get; private set; }
        public string DataPath { get; private set; }
        private string m_Range;
        private string[] m_Languages;
        private Action<string> m_LogCall;
        private List<DataGroup> m_GroupData = new List<DataGroup>();
        private List<string> m_LanKeys = new List<string>();
        private Dictionary<string, string> m_LanDatas = new Dictionary<string, string>();
        public DataGroup this[string name] => m_GroupData.Find(m => m.Name == name);

        public IEnumerable<DataGroup> Groups => m_GroupData;

        public DataManager(Action<string> logcall, string excelPath, string codePath, string dataPath, string range, params string[] languages) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            m_LogCall = logcall;
            m_ExcelPath = excelPath;
            CodePath = codePath;
            DataPath = dataPath;
            m_Range = range;
            m_Languages = languages;
            foreach (var dir in Directory.GetDirectories(m_ExcelPath))
                m_GroupData.Add(new DataGroup(this, dir));
        }

        public void LoadType() {
            foreach (var data in m_GroupData)
                data.LoadType();
        }

        public void CheckType() {
            foreach (var data in m_GroupData)
                data.CheckType();
        }

        public void LoadData() {
            foreach (var data in m_GroupData)
                data.LoadData();
        }

        public void CheckData() {
            foreach (var data in m_GroupData)
                data.CheckData();
        }

        public void ExportData() {
            foreach (var data in m_GroupData)
                data.ExportData();
            ExportLanguageData();
        }

        private void ExportLanguageData() {
            var dir = $"{DataPath}/Languages";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var path = $"{dir}/{m_Languages[0]}.bytes";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate)) {
                using (var bw = new BinaryWriter(fs)) {
                    bw.Write(m_LanKeys.Count);
                    for (int i = 0; i < m_LanKeys.Count; i++)
                        bw.Write(m_LanDatas[m_LanKeys[i]]);
                }
            }
        }

        public void Log(string msg) => m_LogCall?.Invoke(msg);

        public bool CheckRange(string input) {
            if (m_Range == "All" || input == "All" || input == "Key")
                return true;
            return input.Contains(m_Range);
        }

        public bool IsBaseType(string type) => type == "ID" || type == "Num" || type == "Str" || type == "Lan";

        public Func<string, string, object> GetBaseReader(string address, string type) {
            switch (type) {
                case "ID":
                    return ReadID;
                case "Num":
                    return ReadNum;
                case "Str":
                    return ReadStr;
                default:
                    throw new Exception($"{address} Unknow type:{type}.");
            }
        }

        public object ReadID(string address, string value) {
            if (string.IsNullOrEmpty(value))
                return null;
            if (!ulong.TryParse(value, out var id))
                throw new Exception($"{address} Can not convert to ID.");
            return id;

        }

        public int AddLan(string address, string key, string value) {
            if (m_LanDatas.ContainsKey(key))
                throw new Exception($"{address} Language key duplication.");
            m_LanKeys.Add(key);
            m_LanDatas.Add(key, value);
            return m_LanKeys.Count;
        }

        public object ReadNum(string address, string value) {
            if (string.IsNullOrEmpty(value))
                return null;
            if (!int.TryParse(value, out var num))
                throw new Exception($"{address} Can not convert to Number.");
            return num;
        }
        public object ReadStr(string address, string value) {
            if (string.IsNullOrEmpty(value))
                return null;
            return value;
        }
    }
}