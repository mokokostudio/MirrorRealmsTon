using System;

namespace GameEditor.Editors.Domain {
    public class EAPI {
        public virtual void Check() { }
        public static string GetName(EAPIAct obj) {
            if (obj == null)
                return GameEditorStyles.ErrorColor.Dye("未配置");
            return obj.ToString();
        }
        public static string GetName<T>(EAPIReturn<T> obj) {
            if (obj == null)
                return GameEditorStyles.ErrorColor.Dye("未配置");
            if (obj is EStatic<T>)
                return obj.ToString();
            return GameEditorStyles.APIReferenceColor.Dye($"({obj})");
        }
        public static string GetName<T>(EAPIReturnArray<T> obj) {
            if (obj == null)
                return GameEditorStyles.ErrorColor.Dye("未配置");
            if (obj is EArrayEmpty<T>)
                return obj.ToString();
            return GameEditorStyles.APIReferenceColor.Dye($"({obj})");
        }

        public static string GetName<T>(EVariable<T> obj) {
            if (obj == null || string.IsNullOrEmpty(obj.Name))
                return GameEditorStyles.ErrorColor.Dye("未配置");
            return GameEditorStyles.VariableColor.Dye(obj.ToString());
        }

        public static string GetName<T>(EArray<T> obj) {
            if (obj == null || string.IsNullOrEmpty(obj.Name))
                return GameEditorStyles.ErrorColor.Dye("未配置");
            return GameEditorStyles.VariableColor.Dye(obj.ToString());
        }

        public static string GetName<T>(T obj) {
            if (typeof(T).IsEnum)
                return EnumSampleDrawer<T>.GetEnumName(obj);
            if (obj is GameEditorAsset)
                return GameEditorStyles.StaticColor.Dye(obj.ToString());
            else if (obj is Enum)
                return GameEditorStyles.StaticColor.Dye(obj.ToString());
            else
                return GameEditorStyles.StaticColor.Dye(DomainUtil.ValueToString(obj));
        }
    }
}
