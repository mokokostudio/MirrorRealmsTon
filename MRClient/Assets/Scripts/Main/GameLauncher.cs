
   using System.Collections;
using System.Collections.Generic;
   using BDFramework;
   using BDFramework.ScreenView;
   using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
   using UnityEngine.AddressableAssets.ResourceLocators;
   using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameLauncher : MonoBehaviour
{

    public int maxRetryAttempts = 3;
    private AsyncOperationHandle<List<IResourceLocator>> updateHandle;

    private void Start()
    {
       // StartCoroutine(CheckForUpdates());
       
       
    }

    private IEnumerator CheckForUpdates()
    {
        //"检查更新中...";
      

        AsyncOperationHandle<List<string>> checkUpdateHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkUpdateHandle;

        if (checkUpdateHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (checkUpdateHandle.Result.Count > 0)
            {
                //"发现更新，需要下载新资源。";
                // StartCoroutine(DownloadUpdates()));
            }
            else
            {
               //"已是最新版本，无需更新。";
                // 进入游戏
            }
        }
        else
        {
            //"检查更新失败，请重试。";
            //retryAttempts++;
            if (10 <= maxRetryAttempts)
            {
                StartCoroutine(CheckForUpdates());
            }
            else
            {
               //"重试次数已达上限，请检查网络连接。";
            }
        }
    }

    private IEnumerator DownloadUpdates()
    {
       // "下载更新中...";

        updateHandle = Addressables.UpdateCatalogs();
        StartCoroutine(MonitorDownloadProgress());

        while (!updateHandle.IsDone)
        {
            if (updateHandle.Status == AsyncOperationStatus.Failed)
            {
                //retryAttempts++;
                if (10<= maxRetryAttempts)
                {
                    // $"下载失败，正在尝试重连（{retryAttempts}/{maxRetryAttempts}）...";
                    yield return new WaitForSeconds(1);
                    updateHandle = Addressables.UpdateCatalogs();
                    StartCoroutine(MonitorDownloadProgress());
                }
                else
                {
                    //"重试次数已达上限，请检查网络连接。";
                    break;
                }
            }
            yield return null;
        }

        if (updateHandle.Status == AsyncOperationStatus.Succeeded)
        {
          //"更新完成。";
          
            // 进入游戏
        }
    }

    private IEnumerator MonitorDownloadProgress()
    {
        while (!updateHandle.IsDone)
        {
           // progressText.text = $"{(updateHandle.PercentComplete * 100):0}%";
            yield return null;
        }
    }
}

