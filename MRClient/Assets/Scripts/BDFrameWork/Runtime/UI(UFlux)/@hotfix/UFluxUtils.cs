using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BDFramework.ResourceMgr;
using BDFramework.UFlux.View.Props;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BDFramework.UFlux
{
    static public partial class UFluxUtils
    {
        #region 组件初始化

        /// <summary>
        /// 组件类缓存
        /// </summary>
        public class ComponentClassCache
        {
            public FieldInfo[] FieldInfos;
            public PropertyInfo[] PropertyInfos;
            public MethodInfo[] MethodInfos;
        }


        /// <summary>
        /// Component 类数据缓存
        /// </summary>
        static Dictionary<string, ComponentClassCache> ComponentClassCacheMap =
            new Dictionary<string, ComponentClassCache>();

        /// <summary>
        /// 绑定Windows的值
        /// </summary>
        /// <param name="o"></param>
        static public void InitComponent(IComponent component)
        {
            var comType = component.GetType();

            ComponentClassCache classCache = null;

            //缓存各种Component的class数据
            if (!ComponentClassCacheMap.TryGetValue(comType.FullName, out classCache))
            {
                var fields = comType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                var properties =
                    comType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                var methodes = comType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                //筛选有属性的,且为自动赋值的
                classCache = new ComponentClassCache();
                classCache.FieldInfos = fields.Where((f) =>
                    {
                        var attrs = f.GetCustomAttributes(false);
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            if (attrs[i] is AutoInitComponentAttribute)
                                return true;
                        }

                        return false;
                    })
                    .ToArray();
                classCache.PropertyInfos = properties.Where((p) =>
                    {
                        var attrs = p.GetCustomAttributes(false);
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            if (attrs[i] is AutoInitComponentAttribute)
                                return true;
                        }

                        return false;
                    })
                    .ToArray();
                classCache.MethodInfos = methodes.Where((m) =>
                    {
                        var attrs = m.GetCustomAttributes(false);
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            if (attrs[i] is AutoInitComponentAttribute)
                                return true;
                        }

                        return false;
                    })
                    .ToArray();
                //缓存cls data
                ComponentClassCacheMap[comType.FullName] = classCache;
            }

            //开始赋值逻辑
            foreach (var f in classCache.FieldInfos)
            {
                var attrs = f.GetCustomAttributes(false);
                for (int i = 0; i < attrs.Length; i++)
                {
                    (attrs[i] as AutoInitComponentAttribute)?.AutoSetField(component, f);
                }
            }

            foreach (var p in classCache.PropertyInfos)
            {
                var attrs = p.GetCustomAttributes(false);
                for (int i = 0; i < attrs.Length; i++)
                {
                    (attrs[i] as AutoInitComponentAttribute)?.AutoSetProperty(component, p);
                }
            }

            foreach (var m in classCache.MethodInfos)
            {
                var attrs = m.GetCustomAttributes(false);
                for (int i = 0; i < attrs.Length; i++)
                {
                    (attrs[i] as AutoInitComponentAttribute)?.AutoSetMethod(component, m);
                }
            }
        }

        #endregion

        #region 组件值绑定

        /// <summary>
        /// 设置Component Props
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="aState"></param>
        static public void SetComponentProps(Transform trans, APropsBase props)
        {
            props.Transform = trans;
            ComponentBindAdaptorManager.Inst.SetTransformProps(trans, props);
        }

        #endregion

        #region 资源相关操作

        /// <summary>
        /// 加载接口
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        static public T LoadAsync<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>($"Assets/{path}");
            handle.WaitForCompletion();
            return handle.Result;
        }

        static public Sprite LoadIcon(string name)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>($"Assets/Icon/{name}.png");
            return handle.WaitForCompletion();
        }

        static public Sprite LoadSprite(string name) {
            var handle = Addressables.LoadAssetAsync<Sprite>($"{name}.png");
            return handle.WaitForCompletion();
        }

        // static public Dictionary<string, SceneInstance> SceneDic = new Dictionary<string, SceneInstance>();
        static public List<AsyncOperationHandle> TaskList = new List<AsyncOperationHandle>();

        static public Dictionary<string, AsyncOperationHandle<SceneInstance>> SceneDic =
            new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        /// <summary>
        /// 加载接口
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        static public void AsyncLoad<T>(string path, Action<T> callback) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>($"Assets/UI/{path}.prefab");
            TaskList.Add(handle);
            handle.Completed += (m) =>
            {
                if (m.Status == AsyncOperationStatus.Succeeded)
                {
                    callback.Invoke(m.Result);
                }
            };
        }

        static public AsyncOperationHandle<T> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>($"Assets/{path}");
            TaskList.Add(handle);
            return handle;
        }

        static public async UniTask<T> LoadAssetUniTask<T>(string path) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                T asset = handle.Result;
                return asset;
            }
            else
            {
                Debug.LogError("Failed to load asset: " + handle.OperationException);
                return null;
            }
        }

        static public async UniTask AsyncLoadScene(string sceneName)
        {
            if (SceneDic.ContainsKey(sceneName))
                SceneDic.Remove(sceneName);
            var handle = Addressables.LoadSceneAsync($"Assets/Scene/{sceneName}.unity", LoadSceneMode.Additive);
            await handle;
            SceneManager.SetActiveScene(handle.Result.Scene);
            SceneDic.Add(sceneName, handle);
            // SceneDic.Add("BattleTest", handle.Result);
            TaskList.Add(handle);
        }

        static public async UniTask AsyncUnLoadkScene(string sceneName)
        {
            if (SceneDic.ContainsKey(sceneName))
            {
                //await SceneManager.UnloadSceneAsync(SceneDic[sceneName].Result.Scene);
                await Addressables.UnloadSceneAsync(SceneDic[sceneName]);
                // SceneDic.Add("BattleTest", handle.Result);
               // TaskList.Add(handle);
                SceneDic.Remove(sceneName);
            }
        }

        static public void UnAsyncLoadScene()
        {
        }

        /// <summary>
        /// 实例化接口
        /// </summary>
        /// <param name="go"></param>
        static public void Instantiate(GameObject go)
        {
        }


        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="go"></param>
        static public void Destroy(GameObject go)
        {
            if (go)
            {
                Addressables.ReleaseInstance(go);
                GameObject.Destroy(go);
            }
        }

        /// <summary>
        /// 卸载，ab中需要
        /// </summary>
        /// <param name="path"></param>
        static public void Unload(string path)
        {
        }

        #endregion
    }
}