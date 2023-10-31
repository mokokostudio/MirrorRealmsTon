using MR.Battle.Timeline;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

[CustomEditor(typeof(AttackBoxPlayableAsset))]
[CanEditMultipleObjects]
public class AttackBoxPlayableAssetEditor : OdinEditor {

    public AttackBoxPlayableAsset m_Asset;

    protected override void OnEnable() {
        m_Asset = serializedObject.targetObject as AttackBoxPlayableAsset;
        SceneView.duringSceneGui += SceneView_duringSceneGui;
    }

    protected override void OnDisable() {
        SceneView.duringSceneGui -= SceneView_duringSceneGui;
    }
    private void SceneView_duringSceneGui(SceneView obj) {
        if (!m_Asset.isPlaying)
            return;

        Handles.matrix = m_Asset.ownerTr.localToWorldMatrix;
        var p = m_Asset.position;
        var r = Quaternion.Euler(m_Asset.rotation);
        var s = m_Asset.scale;
        Handles.TransformHandle(ref p, ref r, ref s);
        var e = r.eulerAngles;
        if (p != m_Asset.position || e != m_Asset.rotation || s != m_Asset.scale) {
            Undo.RecordObject(m_Asset, "Transform");
            m_Asset.position = p;
            m_Asset.rotation = e;
            m_Asset.scale = s;
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
            EditorUtility.SetDirty(m_Asset);
        }
        Handles.matrix *= Matrix4x4.TRS(p, r, s);
        Handles.color = Color.green;
        Handles.DrawWireCube(Vector3.zero, Vector3.one);
    }
}