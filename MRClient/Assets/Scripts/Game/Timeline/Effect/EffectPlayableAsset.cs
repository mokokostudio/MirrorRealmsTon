using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class EffectPlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;

        public GameObject target;
        public bool follow;
        public float speed = 1;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;

        [NonSerialized]
        public Transform ownerTr;
        [NonSerialized]
        public bool isPlaying;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            ownerTr = owner.transform;
            var pb = ScriptPlayable<Behaviour>.Create(graph);
            pb.GetBehaviour().Set(owner, this);
            return pb;
        }

        private class Behaviour : PlayableBehaviour {

            private Transform m_Owner;
            private EffectPlayableAsset m_Asset;
            private PlayableDirector m_Director;

            public void Set(GameObject owner, EffectPlayableAsset asset) {
                m_Owner = owner.transform;
                m_Asset = asset;
            }

            public override void OnBehaviourPlay(Playable playable, FrameData info) {
                m_Asset.isPlaying = true;
                GameObject go = Instantiate(m_Asset.target);
                go.hideFlags = HideFlags.DontSave;
                m_Director = go.GetComponent<PlayableDirector>();
                ApplayMatrix();
            }

            public override void OnBehaviourPause(Playable playable, FrameData info) {
                m_Asset.isPlaying = false;
                if (!m_Director)
                    return;
                DestroyImmediate(m_Director.gameObject);
            }

            public override void PrepareFrame(Playable playable, FrameData info) {
                var time = (float)playable.GetTime() * m_Asset.speed;
                m_Director.time = time;
                m_Director.Evaluate();
                ApplayMatrix();
            }

            private void ApplayMatrix() {
                Transform tr = m_Director.transform;
                tr.SetParent(m_Owner, false);
                tr.localPosition = m_Asset.position;
                tr.localEulerAngles = m_Asset.rotation;
                tr.localScale = m_Asset.scale;
            }
        }
    }
}