using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniTaskTutorial.Test
{
    public class TaskExecuter : MonoBehaviour
    {
        public bool flag;

        public bool doForget;
        public bool doCoroutine;
        private CancellationTokenSource _cancellationToken;

        private void Start()
        {
            _cancellationToken = new CancellationTokenSource();
        }

        public void TestUniTask()
        {
            if (doCoroutine)
            {
                StartCoroutine(RunDelayCoroutine(1));
            }
            else
            {
                if (doForget)
                {
                    RunDelayTask(1).SuppressCancellationThrow().Forget();
                }
                else
                {
                    RunDelayTask(1).SuppressCancellationThrow();
                }
            }
        }

        private IEnumerator RunDelayCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            flag = !flag;
            DoSomething();
        }

        private async UniTask RunDelayTask(float time)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), false, PlayerLoopTiming.Update, _cancellationToken.Token);
            flag = !flag;
            DoSomething();
        }

        private void DoSomething()
        {
            transform.position += Vector3.right;
        }
    }
}