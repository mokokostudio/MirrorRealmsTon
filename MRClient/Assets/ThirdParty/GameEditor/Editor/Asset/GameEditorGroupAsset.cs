using GameEditor.Editors;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GameEditorGroupAsset<T> : GameEditorAsset where T : GameEditorChildAsset {
    private static Regex s_PathRegex = new Regex("(.+)/([0-9]+)/group.asset");
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

    public override string ToString() => $"组 {base.ToString()}";
}
