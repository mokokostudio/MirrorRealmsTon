using UnityEditor;
using UnityEngine;
using ExcelExport;
using UnityEngine.AddressableAssets;

public static class ExcelCommond {

    [MenuItem("Tools/ExcelExport/LoadTest")]
    public static void TestLoad() {
        Config.LoadData(m => {
            var ao = Addressables.LoadAssetAsync<TextAsset>($"Assets/Config/Groups/{m}.bytes");
            ao.WaitForCompletion();
            return ao.Result.bytes;
        });
        var ao2 = Addressables.LoadAssetAsync<TextAsset>($"Assets/Config/Languages/Cn.bytes");
        ao2.WaitForCompletion();
        Config.LoadLanguage(ao2.Result.bytes);
        Debug.Log("Load complete.");
    }

    [MenuItem("Tools/ExcelExport/Gen", priority = 51)]
    public static void ExportAll() {
        var manager = CreateManager();
        manager.LoadType();
        Debug.Log("Load types complete.");
        manager.CheckType();
        Debug.Log("Check types complete.");
        manager.LoadData();
        Debug.Log("Load Datas complete.");
        manager.CheckData();
        Debug.Log("Check Datas complete.");
        manager.ExportData();
        Debug.Log("Export Datas complete.");
        CSExporter.Export(manager);
        Debug.Log("Export CS complete.");
    }

    private static DataManager CreateManager() => new DataManager(
        m => Debug.Log(m),
        "Excel/Tables",
        "Assets/Scripts/Config/Gen",
        "Assets/ABAssets/Config",
        "Client",
        "Cn",
        "En",
        "Jp",
        "Kr");
}