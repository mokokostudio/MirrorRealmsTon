﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BDFramework.ScreenView
{
    /// <summary>
    /// IScreenView接口
    /// </summary>
    public interface IScreenView
    {
        /// <summary>
        /// IScreenView名称
        /// </summary>
        int Name { get; set; }
        /// <summary>
        /// IScreenView是否加载
        /// </summary>
        bool IsLoad { get; }
        /// <summary>
        /// IScreenView 初始化
        /// </summary>
        void BeginInit(); 
        /// <summary>
        /// IScreenView 退出
        /// </summary>
        void BeginExit();
        
    }
}