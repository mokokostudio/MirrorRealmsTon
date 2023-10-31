using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;
using TrueSync;

namespace MR.Battle.Timeline {
    [Serializable]
    public class BulletPlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;

        public Vector3 position;
        public Vector3 rotation;

        [Range(0, 3)]
        public int atkLv;
        public HitType hitType;

        public int sp;
        public int hp;

        public bool faceTarget;
        public FP speed;
        public FP trackDeg;
        public FP trackTime;

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
            private BulletPlayableAsset m_Asset;

            public void Set(BulletPlayableAsset asset) {
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