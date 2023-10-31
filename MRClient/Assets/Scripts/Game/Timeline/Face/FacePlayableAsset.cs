using System;
using UnityEngine.Playables;
using UnityEngine;

namespace MR.Battle.Timeline {
    [Serializable]
    public class FacePlayableAsset : PlayableAsset {
        public bool lockTarget;
        public float maxAngle;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}