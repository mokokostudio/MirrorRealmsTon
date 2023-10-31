using BDFramework.UFlux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour {
    private static BGMManager s_Instance;

    public static void Play(BGMType type) {
        if (s_Instance == null)
            Instantiate(UFluxUtils.LoadAsync<GameObject>("Logic/BGMManager.prefab"));
        s_Instance.PlayIns(type);
    }

    public AudioClip login;
    public AudioClip main;
    public AudioClip fight;
    public AudioClip fight2;

    private List<AudioSource> m_Sources = new List<AudioSource>();
    private BGMType m_CurrentType;

    private void Awake() {
        s_Instance = this;
        DontDestroyOnLoad(this);
    }

    private void PlayIns(BGMType type) {
        if (m_CurrentType == type)
            return;
        m_CurrentType = type;
        var go = new GameObject("BGM");
        go.transform.SetParent(transform);
        var source = go.AddComponent<AudioSource>();
        source.loop = true;
        switch (type) {
            case BGMType.Login:
                source.clip = login;
                break;
            case BGMType.Main:
                source.clip = main;
                break;
            case BGMType.Fight:
                source.clip = fight;
                break;
            case BGMType.Fight2:
                source.clip = fight2;
                break;
        }
        source.Play();
        m_Sources.Add(source);
    }

    private void Update() {
        for (int i = 0; i < m_Sources.Count; i++) {
            var source = m_Sources[i];
            if (i < m_Sources.Count - 1) {
                source.volume -= Time.deltaTime;
                if (source.volume <= 0) {
                    m_Sources.RemoveAt(i--);
                    Destroy(source.gameObject);
                }
            } else {
                if (source.volume < 0)
                    source.volume = Mathf.Min(1, source.volume + Time.deltaTime);
            }
        }
    }

    public enum BGMType {
        None,
        Login,
        Main,
        Fight,
        Fight2
    }
}
