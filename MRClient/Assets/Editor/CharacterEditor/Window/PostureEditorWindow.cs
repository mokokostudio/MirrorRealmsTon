using GameEditor.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PostureEditorWindow : GameEditorWindow {
    protected override Type AssetType => typeof(PostureAsset);
    [MenuItem("Tools/GameEditors/Posture _F1", false, 53)]
    public static void Open() => CreateWindow<PostureEditorWindow>().Focus();
}
