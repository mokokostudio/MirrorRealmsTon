﻿using BDFramework;
using System.Collections.Generic;
using UnityEngine;
using System;
using BDFramework.Mgr;
using BDFramework.GameStart;
using UnityEngine.EventSystems;


public class BDLauncherBridge
{
    static private IHotfixGameStart hotfixStart = null;
    public static Dictionary<string, Type> UIComponentTypes { get; set; } = new Dictionary<string, Type>();
    /// <summary>
    /// 整个游戏的启动器
    /// </summary>
    /// <param name="mainProjectTypes">游戏逻辑域传过来的所有type</param>
    static public void Start(Type[] mainProjectTypes = null, Type[] hotfixTypes = null)
    {
        //UI组件类型注册
        //ui类型
        Debug.Log("注册UI");
        var uitype = typeof(UIBehaviour);
       Debug.Log($"ui数量：{mainProjectTypes.Length}");
        for (int i = 0; i < mainProjectTypes.Length; i++)
        {
            var type = mainProjectTypes[i];
            //注册所有uiComponent
            bool ret = type.IsSubclassOf(uitype);
            // if (ret)
            // {
            //     if (!ILRuntimeHelper.UIComponentTypes.ContainsKey(type.Name))
            //     {
            //         //因为Attribute typeof（Type）后无法获取fullname
            //         ILRuntimeHelper.UIComponentTypes[type.FullName] = type;
            //     }
            //     else
            //     {
            //         BDebug.LogError("有重名UI组件，请注意" + type.FullName);
            //     }
            // }
        }



        // //执行热更逻辑
        // if (hotfixTypes != null)
        // {
        //     TriggerHotFixGameStart(hotfixTypes);
        //     //获取管理器列表，开始工作
        //     BDebug.Log("热更Instance初始化...",Color.red);
        //     var hotfixMgrList = ILRuntimeManagerInstHelper.LoadManagerInstance(hotfixTypes);
        //     //启动热更管理器
        //     foreach (var hotfixMgr in hotfixMgrList)
        //     {
        //         hotfixMgr.Start();
        //     }
        // }
        // else
        
            //热更逻辑为空,触发主工程的GameStart
          //  TriggerHotFixGameStart(mainProjectTypes);
            //启动著工程的管理器
            ManagerInstHelper.Start();
        
    }

    /// <summary>
    /// 热更启动
    /// </summary>
    /// <param name="types"></param>
    static private void TriggerHotFixGameStart(Type[] types)
    {
        //寻找IGameStart
        for (int i = 0; i < types.Length; i++)
        {
            // 游戏启动器
            var type = types[i];
            if (!type.IsClass) continue;

            var interfaceTypes = type.GetInterfaces();
            for (int j = 0; j < interfaceTypes.Length; j++)
            {
                var interfaceType = interfaceTypes[j];
                if (interfaceType.Name.Contains(nameof(IHotfixGameStart)))
                {
                    hotfixStart = Activator.CreateInstance(type) as IHotfixGameStart;
                    break;
                }
            }
        }

        //gamestart生命注册
        if (hotfixStart != null)
        {
            hotfixStart.Start();
            BDLauncher.OnUpdate += hotfixStart.Update;
            BDLauncher.OnLateUpdate += hotfixStart.LateUpdate;
        }
    }
}