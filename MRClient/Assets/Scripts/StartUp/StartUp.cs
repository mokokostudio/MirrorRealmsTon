using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class StartUp : MonoBehaviour {
    public Slider slider;
    private IEnumerator Start() {

        //var size = Addressables.GetDownloadSizeAsync("all");

        var download = Addressables.DownloadDependenciesAsync("all");
        while (!download.IsDone) {
            slider.value = download.PercentComplete;
            yield return null;
        }
        slider.value = 1;

        yield return StartUpUtil.LoadAssembly("HotUpdate");

        var ao = Addressables.LoadAssetAsync<GameObject>("Assets/HotUpdate.prefab");
        yield return ao;
        var go = Instantiate(ao.Result);
        while (go)
            yield return null;
        Destroy(gameObject);
    }
}
