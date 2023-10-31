using Sirenix.OdinInspector;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public abstract class GameEditorChildAsset : GameEditorAsset {

        private static Regex s_PathRegex = new Regex(".+/([0-9]+).asset");
        private static Regex s_ParentRegex = new Regex(".+/([0-9]+)/[a-zA-Z]+/[0-9]+.asset");
        private static Regex s_GroupRegex = new Regex("(.+)/[0-9]+.asset");

        [IDPreview]
        [ShowInInspector]
        [FoldoutGroup("基础"), LabelText("ID")]
        public int ID {
            get {
                if (IDRuleConfig.Parse == null)
                    return AssetNumber;
                string idStr = "";
                foreach (char c in IDRuleConfig.Parse) {
                    switch (c) {
                        case 'P':
                        case 'p':
                            idStr += GetParent().AssetNumberString;
                            break;
                        case 'G':
                        case 'g':
                            idStr += GetGroup().AssetNumberString;
                            break;
                        case 'N':
                        case 'n':
                            idStr += AssetNumberString;
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            idStr += c;
                            break;
                    }
                }
                return int.Parse(idStr);
            }
        }

        public GameEditorParentAsset GetParent() {
            string assetPath = AssetDatabase.GetAssetPath(this);
            var group = s_ParentRegex.Match(assetPath).Groups[1];
            string path = $"{assetPath.Remove(group.Index + group.Length)}/data.asset";
            return AssetDatabase.LoadAssetAtPath<GameEditorParentAsset>(path);
        }

        public GameEditorAsset GetGroup() {
            string assetPath = AssetDatabase.GetAssetPath(this);
            var group = s_GroupRegex.Match(assetPath).Groups[1];
            string path = $"{assetPath.Remove(group.Index + group.Length)}/group.asset";
            return AssetDatabase.LoadAssetAtPath<GameEditorAsset>(path);
        }

        public override void Delete() => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));

        public override int GetAssetNumber() => int.Parse(s_PathRegex.Match(AssetDatabase.GetAssetPath(this)).Groups[1].Value);
        public override void SetAssetNumber(int number) {
            var tar = string.Format(AssetReplacePath, number);
            if (!AssetDatabase.LoadAssetAtPath<Object>(tar))
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(this), tar);
        }

        public string AssetReplacePath {
            get {
                var org = AssetDatabase.GetAssetPath(this);
                var group = s_PathRegex.Match(org).Groups[1];
                return org.Remove(group.Index, group.Length).Insert(group.Index, $"{{0:{NumberConfig.Format}}}");
            }
        }
    }
}
