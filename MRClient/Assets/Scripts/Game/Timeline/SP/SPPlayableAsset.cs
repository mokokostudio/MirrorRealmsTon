using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class SPPlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;

        public int value;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}