using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Main {

    public bool IsLoading => m_LoadingUI.gameObject.activeInHierarchy;

    private Transform m_UIRoot;
    private LoadingUI m_LoadingUI;
    private Transform m_UIStage;

    private GameObject m_Stagego;

    private void InitUI() {
        m_UIRoot = transform.Find("UIRoot");
        m_UIStage = m_UIRoot.Find("Adaption/Stage");
        m_LoadingUI = m_UIRoot.Find("Adaption/Loading").GetComponent<LoadingUI>();
        m_LoadingUI.Close();
    }

    private void ReplaceUIStage(GameObject go) {
        if (m_Stagego)
            Destroy(m_Stagego);
        m_Stagego = Instantiate(go, m_UIStage);
    }
}
