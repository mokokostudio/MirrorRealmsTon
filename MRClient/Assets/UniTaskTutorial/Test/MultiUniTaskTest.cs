using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace UniTaskTutorial.Test
{
    public class MultiUniTaskTest : MonoBehaviour
    {
        public int Count = 10000;
        public bool DoForget;
        public bool DoCoroutine;
        private TaskExecuter[] _executers;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 120, 30), "创建"))
            {
                if (_executers != null)
                {
                    foreach (TaskExecuter executer in _executers)
                    {
                        Destroy(executer.gameObject);
                    }
                }
                _executers = new TaskExecuter[Count];
                for (int i = 0; i < Count; i++)
                {
                    var go = new GameObject(i.ToString());
                    _executers[i] = go.AddComponent<TaskExecuter>();
                }
                
            }
            
            if (GUI.Button(new Rect(0, 40, 120, 30), "测试"))
            {

                Profiler.BeginSample("testUniTask");
                for (int i = 0; i < Count; i++)
                {
                    _executers[i].doForget = DoForget;
                    _executers[i].doCoroutine = DoCoroutine;
                    _executers[i].TestUniTask();
                }
                Profiler.EndSample();
                
                Debug.Break();
                
            }
        }
    }
}