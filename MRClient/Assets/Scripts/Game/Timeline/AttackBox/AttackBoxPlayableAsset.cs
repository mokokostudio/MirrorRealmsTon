using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class AttackBoxPlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;

        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;

        [Range(0, 3)]
        public int atkLv;
        public HitType hitType;

        public int sp;
        public int hp;

        public int paush;

        [NonSerialized]
        public Transform ownerTr;
        [NonSerialized]
        public bool isPlaying;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            ownerTr = owner.transform;
            var pb = ScriptPlayable<Behaviour>.Create(graph);
            pb.GetBehaviour().Set(this);
            return pb;
        }

        private class Behaviour : PlayableBehaviour {
            private AttackBoxPlayableAsset m_Asset;

            public void Set(AttackBoxPlayableAsset asset) {
                m_Asset = asset;
            }

            public override void OnBehaviourPlay(Playable playable, FrameData info) {
                m_Asset.isPlaying = true;
            }

            public override void OnBehaviourPause(Playable playable, FrameData info) {
                m_Asset.isPlaying = false;
            }
        }
    }
}