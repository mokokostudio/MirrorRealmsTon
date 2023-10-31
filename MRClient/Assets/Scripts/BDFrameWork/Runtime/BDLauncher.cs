using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using BDFramework.GameStart;
using BDFramework.Mgr;

using UnityEngine;
using UnityEngine.AddressableAssets;


public class BDLauncher : MonoBehaviour
{
    /// <summary>
    /// 框架版本号
    /// </summary>
    public const string Version = "2.2.0-preview.2";
    


    #region 对外的生命周期

    public delegate void GameLauncherDelegate();

    static public GameLauncherDelegate OnUpdate { get; set; }
    static public GameLauncherDelegate OnLateUpdate { get; set; }

    #endregion

    static public BDLauncher Inst { get; private set; }

    // Use this for initialization
    private void Awake()
    {
        Debug.Log($"启动框架");
        Inst = this;
        //添加组件
        this.gameObject.AddComponent<IEnumeratorTool>();
        //添加不删除的组件
        if (Application.isPlaying)
        {
            DontDestroyOnLoad(this);
        }
        Launch();
    }

    /// <summary>

    #region 启动热更逻辑

    /// <summary>
    /// 初始化
    /// 修改版本,让这个启动逻辑由使用者自行处理
    /// </summary>
    /// <param name="mainProjectTypes">Editor模式下,UPM隔离了DLL需要手动传入</param>
    /// <param name="GameId">单游戏更新启动不需要id，多游戏更新需要id号</param>
    public void Launch(Action launchSuccessCallback = null)
    {
        Debug.Log("【Launch】Persistent:" + Application.persistentDataPath);
        Debug.Log("【Launch】StreamingAsset:" + Application.streamingAssetsPath);

        //list
        var types = ManagerInstHelper.GetMainProjectTypes();
        //主工程启动
        IGameStart mainStart;
        foreach (var type in types)
        {
            //TODO 这里有可能先访问到 IGamestart的Adaptor
            if (type.IsClass && type.GetInterface(nameof(IGameStart)) != null)
            {
                Debug.Log("【Launch】主工程 Start： " + type.FullName);
                mainStart = Activator.CreateInstance(type) as IGameStart;
                if (mainStart != null)
                {
                    //注册
                    mainStart.Start();
                    OnUpdate += mainStart.Update;
                    OnLateUpdate += mainStart.LateUpdate;
                    break;
                }
            }
        }
        //执行主工程逻辑
        ManagerInstHelper.Load(types);
        //开始资源检测
        Debug.Log("【Launch】框架资源版本验证!");
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetType("BDLauncherBridge");
            Debug.Log($"启动Start");
            var method = type.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
            var list = new List<Type>();
            list.AddRange(this.GetType().Assembly.GetTypes());
            list.AddRange(typeof(BDLauncher).Assembly.GetTypes());
            method.Invoke(null, new object[] {list.ToArray(), null});
        }
        launchSuccessCallback?.Invoke();
    }

    #endregion

    #region 生命周期

    //普通帧循环
    private void Update()
    {
        OnUpdate?.Invoke();
    }

    //更快的帧循环
    private void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    void OnApplicationQuit()
    {
        // #if UNITY_EDITOR
        //             SqliteLoder.Close();
        //             ILRuntimeHelper.Dispose();
        // #endif
    }

    #endregion
}