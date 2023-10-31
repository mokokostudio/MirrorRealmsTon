using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HotUpdate : MonoBehaviour {
    public List<string> HotDlls;
    public List<string> PathAOTs;
    private IEnumerator Start() {
        foreach(var assembly in HotDlls)
            yield return StartUpUtil.LoadAssembly(assembly);
        foreach (var assembly in PathAOTs)
            yield return StartUpUtil.LoadAOT(assembly);
        var go = Addressables.LoadAssetAsync<GameObject>("Assets/Root.prefab");
        yield return go;
        Instantiate(go.Result);
        Destroy(gameObject);
    }
}
