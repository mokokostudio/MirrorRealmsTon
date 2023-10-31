using MR.Battle.Timeline;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

[CustomEditor(typeof(BulletPlayableAsset))]
[CanEditMultipleObjects]
public class BulletPlayableAssetEditor : OdinEditor {

    public BulletPlayableAsset m_Asset;

    protected override void OnEnable() {
        m_Asset = serializedObject.targetObject as BulletPlayableAsset;
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
        p = Handles.PositionHandle(p, r);
        r = Handles.RotationHandle(r, p);
        var e = r.eulerAngles;
        if (p != m_Asset.position || e != m_Asset.rotation) {
            Undo.RecordObject(m_Asset, "Transform");
            m_Asset.position = p;
            m_Asset.rotation = e;
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
            EditorUtility.SetDirty(m_Asset);
        }
        Handles.matrix *= Matrix4x4.TRS(p, r, Vector3.one);
        Handles.color = Color.green;
        //Handles.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
