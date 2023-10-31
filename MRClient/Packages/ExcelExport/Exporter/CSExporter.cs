using System;
using System.IO;

namespace ExcelExport {
    public static class CSExporter {
        private const string ENUM_TYPE_FILE = @"
public static partial class Config {{
    public static partial class {0} {{
        public enum {1} {{{2}
        }}
    }}
}}";
        private const string ENUM_OPTION_STR = @"
            {0},";

        private const string STRUCT_TYPE_FILE = @"
public static partial class Config {{
    public static partial class {0} {{
        public class {1} {{{2}
            internal {1}(Loader loader) {{{3}
            }}
        }}
    }}
}}";
        private const string SINGLE_FIELD_STR = @"
            public {0} {1} {{ get; private set; }}";

        private const string TABLE_FILE = @"
using System.Collections.Generic;
public static partial class Config {{
    public static partial class {0} {{
        public class {1}Data {{{2}
            internal {1}Data(Loader loader) {{{3}
            }}
        }}
    }}
}}";
        private const string MUTI_FIELD_STR = @"
            public List<{0}> {1} {{ get; private set; }}";

        private const string LANGUAGE_FILE = @"
public static partial class Config {{
    public static partial class {0} {{
        public class LanguageData {{{1}
            internal LanguageData(Loader loader) {{{2}
            }}
        }}
    }}
}}";
        private const string LANGUAGE_FIELD = @"
            public LanReference {0} {{ get; private set; }}";


        private const string FIELD_LOAD_ID = @"
                {0} = loader.ReadULong();";
        private const string FIELD_LOAD_NUM = @"
                {0} = loader.ReadInt();";
        private const string FIELD_LOAD_STR = @"
                {0} = loader.ReadString();";
        private const string FIELD_LOAD_LAN = @"
                {0} = loader.ReadInt();";
        private const string FIELD_LOAD_ENUM = @"
                {0} = ({1})loader.ReadInt();";
        private const string FIELD_LOAD_STRUCT = @"
                {0} = new {1}(loader);";

        private const string FIELD_LOAD_ID_ARRAY = @"
                {0} = loader.ReadArray(loader.ReadULong);";
        private const string FIELD_LOAD_NUM_ARRAY = @"
                {0} = loader.ReadArray(loader.ReadInt);";
        private const string FIELD_LOAD_STR_ARRAY = @"
                {0} = loader.ReadArray(loader.ReadString);";
        private const string FIELD_LOAD_LAN_ARRAY = @"
                {0} = loader.ReadArray(loader.ReadUInt);";
        private const string FIELD_LOAD_ENUM_ARRAY = @"
                {0} = loader.ReadArray(() => ({1})loader.ReadInt());";
        private const string FIELD_LOAD_STRUCT_ARRAY = @"
                {0} = loader.ReadArray(() => new {1}(loader));";


        private const string GROUP_FILE = @"
using System.Collections.Generic;
public static partial class Config {{
    public static partial class {0} {{{1}
        internal static void Load(Loader loader) {{{2}
            loader.Dispose();
        }}
    }}
}}";
        private const string GROUP_FIELD = @"
        public static {0}Data {0} {{ get; private set; }}";
        private const string Group_TABLE_FIELD_ARRAY = @"
        public static List<{0}Data> {0} {{ get; private set; }}";
        private const string Group_TABLE_FIELD_DIC = @"
        public static Dictionary<{0}, {1}> {2} {{ get; private set; }}";
        private const string Group_TABLE_FIELD_DIC_ADD = "Dictionary<{0}, {1}>";

        private const string GROUP_LOAD = @"
            {0} = new {0}Data(loader);";
        private const string GROUP_Table_LOAD_ARRAY = @"
            {0} = loader.ReadArray(() => new {0}Data(loader));";
        private const string GROUP_Table_LOAD_DIC = @"
            {0} = Turn(loader.ReadArray(() => new {0}Data(loader)), m => m.{1}{2});";
        private const string GROUP_Table_LOAD_DIC_ADD = ", l => Turn(l, m => m.{0}{1})";

        private const string CONFIG_FILE = @"
using System;
public static partial class Config {{
    public static void LoadData(Func<string, byte[]> dataGet) {{{0}
    }}
}}";
        private const string CONFIG_FIELD = @"
        {0}.Load(new Loader(dataGet(""{0}"")));";

        public static void Export(DataManager manager) {
            var lStr = "";
            foreach (var group in manager.Groups) {
                Export(group);
                lStr += string.Format(CONFIG_FIELD, group.Name);
            }
            var result = string.Format(CONFIG_FILE, lStr);
            Write(result, manager.CodePath, "Config");
        }

        public static void Export(DataGroup group) {
            if (group.Enums != null)
                foreach (var t in group.Enums.Types)
                    Export(t);
            if (group.Structs != null)
                foreach (var t in group.Structs.Types)
                    Export(t);
            var fStr = "";
            var lStr = "";
            if (group.Constant != null) {
                Export(group.Constant);
                fStr += string.Format(GROUP_FIELD, "Constant");
                lStr += string.Format(GROUP_LOAD, "Constant");
            }
            if (group.Language != null) {
                Export(group.Language);
                fStr += string.Format(GROUP_FIELD, "Language");
                lStr += string.Format(GROUP_LOAD, "Language");
            }
            foreach (var t in group.Tables) {
                Export(t);
                if (t.Keys.Count == 0) {
                    fStr += string.Format(Group_TABLE_FIELD_ARRAY, t.Name);
                    lStr += string.Format(GROUP_Table_LOAD_ARRAY, t.Name);
                } else {
                    var add = $"{t.Name}Data";
                    for (int i = t.Keys.Count - 1; i > 0; i--)
                        add = string.Format(Group_TABLE_FIELD_DIC_ADD, ConvertType(t.Keys[i]), add);
                    fStr += string.Format(Group_TABLE_FIELD_DIC, ConvertType(t.Keys[0]), add, t.Name);

                    add = "";
                    for (int i = t.Keys.Count - 1; i > 0; i--)
                        add = string.Format(GROUP_Table_LOAD_DIC_ADD, t.Keys[i].Name, add);
                    lStr += string.Format(GROUP_Table_LOAD_DIC, t.Name, t.Keys[0].Name, add);
                }
            }
            var result = string.Format(GROUP_FILE, group.Name, fStr, lStr);
            Write(result, group.Manager.CodePath, group.Name);
        }

        public static void Export(EnumType type) {
            var gName = type.Group.Name;
            var tName = type.Name;
            var optionStr = "";
            foreach (var option in type.Options)
                optionStr += string.Format(ENUM_OPTION_STR, option.Name);
            var result = string.Format(ENUM_TYPE_FILE, gName, tName, optionStr);
            Write(result, $"{type.Manager.CodePath}/{gName}", tName);
        }

        public static void Export(StructType type) {
            var gName = type.Group.Name;
            var tName = type.Name;
            var fieldStr = "";
            var loadStr = "";
            foreach (var field in type.Fields) {
                fieldStr += string.Format(SINGLE_FIELD_STR, ConvertType(field), field.Name);
                loadStr += GetLoadStr(field.Group, false, field.Name, field.Type, field.TypeGroup);
            }
            var result = string.Format(STRUCT_TYPE_FILE, gName, tName, fieldStr, loadStr);
            Write(result, $"{type.Manager.CodePath}/{gName}", tName);
        }

        public static void Export(ConstantData table) {
            var gName = table.Group.Name;
            var fieldStr = "";
            var loadStr = "";
            foreach (var field in table.Fields) {
                fieldStr += string.Format(field.IsArray ? MUTI_FIELD_STR : SINGLE_FIELD_STR, ConvertType(field), field.Name);
                loadStr += GetLoadStr(field.Group, field.IsArray, field.Name, field.Type, field.TypeGroup);
            }
            var result = string.Format(TABLE_FILE, gName, "Constant", fieldStr, loadStr);
            Write(result, $"{table.Manager.CodePath}/{gName}", "Constant");
        }

        public static void Export(CustomTableData table) {
            var gName = table.Group.Name;
            var tName = table.Name;
            var fieldStr = "";
            var loadStr = "";
            foreach (var field in table.Fields) {
                fieldStr += string.Format(field.IsArray ? MUTI_FIELD_STR : SINGLE_FIELD_STR, ConvertType(field), field.Name);
                loadStr += GetLoadStr(field.Group, field.IsArray, field.Name, field.Type, field.TypeGroup);
            }
            var result = string.Format(TABLE_FILE, gName, tName, fieldStr, loadStr);
            Write(result, $"{table.Manager.CodePath}/{gName}", tName);
        }

        public static void Export(LanguageData data) {
            var gName = data.Group.Name;
            var fieldStr = "";
            var loadStr = "";
            foreach (var field in data.Fields) {
                fieldStr += string.Format(LANGUAGE_FIELD, field.Name);
                loadStr += GetLoadStr(null, false, field.Name, "Lan", null);
            }
            var result = string.Format(LANGUAGE_FILE, gName, fieldStr, loadStr);
            Write(result, $"{data.Manager.CodePath}/{gName}", "Language");
        }

        private static string GetLoadStr(DataGroup group, bool isArray, string name, string type, string typeGroup) {
            switch (type) {
                case "ID":
                    return string.Format(isArray ? FIELD_LOAD_ID_ARRAY : FIELD_LOAD_ID, name);
                case "Num":
                    return string.Format(isArray ? FIELD_LOAD_NUM_ARRAY : FIELD_LOAD_NUM, name);
                case "Str":
                    return string.Format(isArray ? FIELD_LOAD_STR_ARRAY : FIELD_LOAD_STR, name);
                case "Lan":
                    return string.Format(isArray ? FIELD_LOAD_LAN_ARRAY : FIELD_LOAD_LAN, name);
            }
            var fg = string.IsNullOrEmpty(typeGroup) ? group : group.Manager[typeGroup];
            var st = string.IsNullOrEmpty(typeGroup) ? type : $"{typeGroup}.{type}";
            if (fg.Enums != null && fg.Enums[type] != null)
                return string.Format(isArray ? FIELD_LOAD_ENUM_ARRAY : FIELD_LOAD_ENUM, name, st);
            if (fg.Structs != null && fg.Structs[type] != null)
                return string.Format(isArray ? FIELD_LOAD_STRUCT_ARRAY : FIELD_LOAD_STRUCT, name, st);
            if (fg[type] != null)
                return string.Format(isArray ? FIELD_LOAD_ID_ARRAY : FIELD_LOAD_ID, name);

            throw new Exception();
        }

        private static string ConvertType(Field field) {
            var group = string.IsNullOrEmpty(field.TypeGroup) ? field.Group : field.Manager[field.TypeGroup];
            if (group[field.Type] != null)
                return "ulong";
            switch (field.Type) {
                case "ID":
                    return "ulong";
                case "Num":
                    return "int";
                case "Str":
                    return "string";
                case "Lan":
                    return "LanReference";
                default:
                    return group == field.Group ? field.Type : $"{field.TypeGroup}.{field.Type}";
            }
        }

        private static string ConvertType(StructField field) {
            var group = string.IsNullOrEmpty(field.TypeGroup) ? field.Group : field.Manager[field.TypeGroup];
            if (group[field.Type] != null)
                return "ulong";
            switch (field.Type) {
                case "ID":
                    return "ulong";
                case "Num":
                    return "int";
                case "Str":
                    return "string";
                case "Lan":
                    return "LanReference";
                default:
                    return group == field.Group ? field.Type : $"{field.TypeGroup}.{field.Type}";
            }
        }

        private static void Write(string content, string dir, string file) {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText($"{dir}/{file}.cs", content);
        }
    }
}
