using HybridCLR;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StartUpUtil {

    public static IEnumerator LoadAssembly(string name) {
        var ao = Addressables.LoadAssetAsync<TextAsset>($"Assets/HotDll/{name}.dll.bytes");
        yield return ao;
        Assembly.Load(ao.Result.bytes);
        Debug.Log($"load {name}.dll");
    }

    public static IEnumerator LoadAOT(string name) {
        var ao = Addressables.LoadAssetAsync<TextAsset>($"Assets/HotDll/{name}.dll.bytes");
        yield return ao;
        RuntimeApi.LoadMetadataForAOTAssembly(ao.Result.bytes, HomologousImageMode.SuperSet);
        Debug.Log($"load {name}.dll<meta>");
    }
}
