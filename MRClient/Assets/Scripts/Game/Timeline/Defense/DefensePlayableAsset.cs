using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class DefensePlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}