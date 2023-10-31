using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AutoSetFBX {
    [MenuItem("Tools/Auto Set FBX")]
    public static void SetFBX() {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
            return;

        foreach (var go in Selection.gameObjects) {
            var path = AssetDatabase.GetAssetPath(go);
            if (string.IsNullOrEmpty(path))
                continue;
            AssetDatabase.RenameAsset(path, go.name.Replace("_Root", "").Replace("GhostSamurai_", ""));
        }

        foreach (var go in Selection.gameObjects) {
            var path = AssetDatabase.GetAssetPath(go);
            if (string.IsNullOrEmpty(path))
                continue;
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer == null)
                continue;
            var animations = importer.clipAnimations.Length == 0 ? importer.defaultClipAnimations : importer.clipAnimations;
            if (animations == null || animations.Length == 0)
                continue;

            var config = animations[0];

            config.name = go.name;
            config.lockRootRotation = true;
            config.keepOriginalOrientation = true;
            config.lockRootHeightY = true;
            config.keepOriginalPositionY = true;
            config.lockRootPositionXZ = false;
            config.keepOriginalPositionXZ = false;

            config.maskType = ClipAnimationMaskType.None;

            importer.clipAnimations = animations;
            importer.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
            importer.SaveAndReimport();
        }
    }
}
