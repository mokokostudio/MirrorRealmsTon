﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BDFramework.DataListener;
using BDFramework.Hotfix.Reflection;
using BDFramework.UFlux.View.Props;
using BDFramework.UFlux.WindowStatus;
using UnityEngine;


namespace BDFramework.UFlux
{
    /// <summary>
    /// Window-Prop基类
    /// 不带Flux Store
    /// </summary>
    /// <typeparam name="TP"></typeparam>
    public class AWindow<TP> : ATComponent<TP>, IWindow, IUIMessage where TP : APropsBase, new()
    {
        /// <summary>
        /// 状态管理
        /// </summary>
        public AStatusListener State { get; private set; } = new StatusListenerService();

        /// <summary>
        /// 是否获得焦点
        /// </summary>
        public bool IsFocus { get; protected set; } = false;

        public AWindow(string path) : base(path)
        {
            RegisterUIMessages();
        }

        public AWindow(Transform transform) : base(transform)
        {
            RegisterUIMessages();
        }


        #region 生命周期

        /// <summary>
        /// 打开
        /// 这里是IWindow的接口
        /// </summary>
        /// <param name="uiMsg"></param>
        public override void Open(UIMsgData uiMsg = null)
        {
            base.Open(uiMsg);
            this.State.TriggerEvent<OnWindowOpen>();
        }

        /// <summary>
        /// 关闭
        /// 这里是IWindow的接口
        /// </summary>
        public override void Close()
        {
            base.Close();
            this.State.TriggerEvent<OnWindowClose>();
        }

        /// <summary>
        /// 获得焦点
        /// 这里是IWindow的接口
        /// </summary>
        public void OnFocus()
        {
            this.IsFocus = true;
            this.State.TriggerEvent<OnWindowFocus>();
        }

        /// <summary>
        /// 丢失焦点
        /// </summary>
        public void OnBlur()
        {
            this.IsFocus = false;
            this.State.TriggerEvent<OnWindowBlur>();
        }

        #endregion

        #region ui消息

        //
        public delegate void UIMessageDelegate(UIMsgData message);

        /// <summary>
        /// Action 回调表
        /// </summary>
        protected Dictionary<string, MethodInfo> msgCallbackMap = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// 注册回调
        /// </summary>
        private void RegisterUIMessages()
        {
            //注册回调
            var t = this.GetType();
            var flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var mi in t.GetMethods(flag))
            {
                var attr = GetAttribute<UIMessageListenerAttribute>(mi);
                if (attr != null)
                {
                    var @params = mi.GetParameters();

                    if (@params.Length == 1)
                    {
                        msgCallbackMap[@params[0].ParameterType.Name] = mi;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("UIMsg 绑定失败，请检查函数签名:" + mi.Name);
                    }
                }
            }
        }

        public T[] GetAttributes<T>(MemberInfo memberInfo) where T : Attribute
        {
            var attrs = memberInfo.GetCustomAttributes<T>(true);
            T[] ts = new T[attrs.Count()];

            var index = 0;
            foreach (var vAttr in attrs)
            {
                if (vAttr is T t)
                {
                    ts[index] = t;
                }

                index++;
            }

            if (ts.Length > 0)
                return ts;
            return null;
        }

        public T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            var attrs = memberInfo.GetCustomAttributes(false);
            foreach (var attr in attrs)
            {
                if (attr is T)
                {
                    return attr as T;
                }
            }

            return null;
        }

        /// <summary>
        /// 更新UI使用的数据
        /// </summary>
        /// <param name="uiMsg">数据</param>
        public void SendMessage(UIMsgData uiMsg)
        {
            //所有的消息会被派发给子窗口
//            var keys = subWindowsMap.Keys.ToList();
//            for (int i = 0; i < keys.Count; i++)
//            {
//                var k = keys[i];
//                subWindowsMap[k].SendMessage(uiMsg);
//            }
            //通知子窗口
            foreach (var subWin in subWindowsMap.Values)
            {
                subWin.SendMessage(uiMsg);
            }

            //TODO: 热更执行完Invoke会导致 map的堆栈出问题，
            MethodInfo method = null;
            var key = uiMsg.GetType();
            bool flag = this.msgCallbackMap.TryGetValue(key.Name, out method);
            if (flag)
            {
                method.Invoke(this, new object[] {uiMsg});
            }
        }

        #endregion

        #region 子窗口

        /// <summary>
        /// 父节点
        /// </summary>
        public IWindow Parent { get; private set; }

        /// <summary>
        /// 子窗口列表
        /// </summary>
        protected Dictionary<int, IWindow> subWindowsMap = new Dictionary<int, IWindow>();

        /// <summary>
        /// 注册窗口
        /// </summary>
        /// <param name="subwin"></param>
        /// <param name="enum"></param>
        public void RegisterSubWindow(IWindow subwin)
        {
            subWindowsMap[subwin.GetHashCode()] = subwin;
            subwin.SetParent(this);
            (subwin as IComponent).Init();
        }


        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="window"></param>
        public void SetParent(IWindow window)
        {
            this.Parent = window;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="enum"></param>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public T1 GetSubWindow<T1>() where T1 : class
        {
            foreach (var value in subWindowsMap.Values)
            {
                if (value is T1)
                {
                    return (T1) value;
                }
            }

            return null;
        }

        #endregion
    }
}