using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public partial class Main : MonoBehaviour {

    public NativeTarget nativeTarget;
    public InputActionAsset inputActionAsset;

    public static Main Instance { get; private set; }
    private Transform m_LogicRoot;

    private void Awake() {
        Application.targetFrameRate = 120;
        Application.runInBackground = true;
//#if UNITY_ANDROID
//        if (Screen.width / Screen.height > 16 / 9)
//            Screen.SetResolution(720 * Screen.width / Screen.height, 720, true);
//        else
//            Screen.SetResolution(1280, 1280 * Screen.height / Screen.width, true);
//#endif
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);

        foreach (var map in inputActionAsset.actionMaps)
            foreach (var action in map)
                action.Enable();

        Config.LoadData(m => {
            var ao = Addressables.LoadAssetAsync<TextAsset>($"Assets/Config/Groups/{m}.bytes");
            ao.WaitForCompletion();
            return ao.Result.bytes;
        });
        var ao2 = Addressables.LoadAssetAsync<TextAsset>($"Assets/Config/Languages/Cn.bytes");
        ao2.WaitForCompletion();
        Config.LoadLanguage(ao2.Result.bytes);
    }

    private void Start() {
        InitUI();
        m_LogicRoot = transform.Find("LogicRoot");

        switch (nativeTarget) {
            case NativeTarget.Normal:
                OpenUIStage("Login");
                break;
            case NativeTarget.RoomTest:
                OpenUIStage("RoomTest");
                break;
        }
    }

    public void OpenUIStage(string name) {
        var handle = Addressables.LoadAssetAsync<GameObject>($"Assets/UI/{name}.prefab");
        m_LoadingUI.AddHandle(handle);
        StartCoroutine(WaitUIStage(handle));
    }

    public void LoadScene(string name) {
        var handle = Addressables.LoadSceneAsync("Assets/Scene/BattleTest.unity");
        m_LoadingUI.Open(handle);
    }

    private IEnumerator WaitUIStage(AsyncOperationHandle<GameObject> ao) {
        yield return ao;
        ReplaceUIStage(ao.Result);
    }

    public AsyncOperationHandle<T> LoadAsset<T>(string path) {
        var handle = Addressables.LoadAssetAsync<T>($"Assets/{path}");
        m_LoadingUI.AddHandle(handle);
        return handle;
    }

    public T LoadAssetSync<T>(string path) {
        var ao = LoadAsset<T>(path);
        ao.WaitForCompletion();
        return ao.Result;
    }

    public Sprite LoadIcon(string name) {
        var handle = Addressables.LoadAssetAsync<Sprite>($"Assets/Icon/{name}.png");
        return handle.WaitForCompletion();
    }

    public enum NativeTarget {
        Normal,
        BattleTest,
        RoomTest
    }
}
