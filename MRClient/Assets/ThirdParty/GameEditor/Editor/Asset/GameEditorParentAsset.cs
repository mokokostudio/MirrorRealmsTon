using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public abstract class GameEditorParentAsset : GameEditorAsset {
        private static Regex s_PathRegex = new Regex("(.+)/([0-9]+)/data.asset");

        public abstract string ExportDirectory { get; }

        public abstract void Export();

        public override void Delete() => AssetDatabase.DeleteAsset(Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)));

        public override int GetAssetNumber() => int.Parse(s_PathRegex.Match(AssetDatabase.GetAssetPath(this)).Groups[2].Value);
        public override void SetAssetNumber(int number) {
            var dir = s_PathRegex.Match(AssetDatabase.GetAssetPath(this)).Groups[1].Value;
            var org = $"{dir}/{AssetNumberString}";
            var tar = $"{dir}/{number.ToString(NumberConfig.Format)}";
            if (AssetDatabase.LoadAssetAtPath<Object>(tar))
                return;
            AssetDatabase.MoveAsset(org, tar);
        }

        public string ChildAssetReplacePath<T>() where T : GameEditorChildAsset {
            var org = AssetDatabase.GetAssetPath(this);
            var dir = Path.GetDirectoryName(org);
            var p = typeof(T).GetAttributeCached<AssetConfigAttribute>().Path;
            var n = typeof(T).GetAttributeCached<NumberRangeAttribute>().Format;
            return $"{dir}/{p}/{{0:{n}}}.asset";
        }

        public List<T> GetChildAssets<T>() where T : GameEditorChildAsset => GameEditorDataUtil.GetAssets<T>(ChildAssetReplacePath<T>());
    }
}
