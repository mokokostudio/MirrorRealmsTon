using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class MovePlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;
        public Vector3 Offset = Vector3.zero;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}