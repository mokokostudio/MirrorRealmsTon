using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Aliyun.OSS;

public static class BuildAssets {
    private const string HOT_DLL_OUT_PATH = "Assets/ABAssets/HotDll";
    private const string ADDRESSABLE_OUT_PATH = "ServerData";

    [MenuItem("Tools/Build/HotAssets Build")]
    public static void ReBuildAssets() {
        CompileDllCommand.CompileDllActiveBuildTarget();

        ClearDir(HOT_DLL_OUT_PATH);
        CopyAOTAssemblies();
        CopyHotUpdateAssemblies();

        AssetDatabase.Refresh();

        ClearDir($"{ADDRESSABLE_OUT_PATH}/{EditorUserBuildSettings.activeBuildTarget}");
        BuildContentUpdate();
    }

    public static void CopyAOTAssemblies() {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);

        foreach (var dll in SettingsUtil.AOTAssemblyNames) {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
            if (!File.Exists(srcDllPath)) {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{HOT_DLL_OUT_PATH}/{dll}.dll.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[CopyAOTAssemblies] copy AOT dll {srcDllPath} -> {dllBytesPath}");
        }
    }

    public static void CopyHotUpdateAssemblies() {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);

        foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved) {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{HOT_DLL_OUT_PATH}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[CopyHotUpdateAssemblies] copy hotfix dll {dllPath} -> {dllBytesPath}");
        }
    }

    public static void BuildContentUpdate() {
        var path = Path.GetDirectoryName(Application.dataPath) + "/" + Addressables.LibraryPath + PlatformMappingService.GetPlatformPathSubFolder() + "/addressables_content_state.bin";
        ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
    }

    public static void ClearDir(string path) {
        Directory.Delete(path, true);
        Directory.CreateDirectory(path);
    }

    [MenuItem("Tools/Build/HotAssets SyncCloud")]
    public static void SyncCloudAssets() {
        OssClient client = new OssClient("oss-ap-southeast-1.aliyuncs.com", "LTAI5tGzUxYSsbNLNiRrsQar", "1vucBBdbPJgg37DZRvuu4awewD5zB0");
        string bucketName;
        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.Android:
                bucketName = "mirror-realms-app-hotdata-android";
                break;
            case BuildTarget.iOS:
                bucketName = "mirror-realms-app-hotdata-ios";
                break;
            default:
                bucketName = "mirror-realms-app-hotdata-win";
                break;
        }

        var oldList = new List<string>();
        foreach (var s in client.ListObjects(bucketName).ObjectSummaries)
            oldList.Add(s.Key);

        foreach (var filePath in Directory.GetFiles($"{ADDRESSABLE_OUT_PATH}/{EditorUserBuildSettings.activeBuildTarget}")) {
            var fileName = Path.GetFileName(filePath);
            client.PutObject(bucketName, fileName, filePath);
            oldList.Remove(fileName);
        }

        if (oldList.Count > 0)
            client.DeleteObjects(new DeleteObjectsRequest(bucketName, oldList));

        Debug.Log("SyncCloud Finish.");
    }

    [MenuItem("Tools/Build/HotAssets All")]
    public static void BuildAll() {
        ReBuildAssets();
        SyncCloudAssets();
    }
}
