using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public class GameEditorCustomAsset : SerializedScriptableObject {
        private static Regex s_ParentRegex = new Regex(".+/([0-9]+)/[a-zA-Z]+/.+.asset");

        public GameEditorParentAsset GetParent() {
            string assetPath = AssetDatabase.GetAssetPath(this);
            var group = s_ParentRegex.Match(assetPath).Groups[1];
            string path = $"{assetPath.Remove(group.Index + group.Length)}/data.asset";
            return AssetDatabase.LoadAssetAtPath<GameEditorParentAsset>(path);
        }
    }
}