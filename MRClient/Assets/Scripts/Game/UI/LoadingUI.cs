using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {
    public Slider slider;

    private List<AsyncOperationHandle> m_targets = new List<AsyncOperationHandle>();
    private int m_Delay;

    private void Update() {
        var a = 0f;
        foreach (var target in m_targets)
            a += target.PercentComplete;
        a /= m_targets.Count;
        slider.value = Mathf.MoveTowards(slider.value, a, Time.deltaTime);
        if (slider.value == 1 && (m_Delay -= 1) < 0)
            Close();
    }

    public void AddHandle(AsyncOperationHandle target) {
        if (!gameObject.activeInHierarchy)
            return;
        m_targets.Add(target);
    }

    public void Open(AsyncOperationHandle target) {
        gameObject.SetActive(true);
        m_targets.Clear();
        m_targets.Add(target);
        slider.value = 0;
        m_Delay = 10;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
