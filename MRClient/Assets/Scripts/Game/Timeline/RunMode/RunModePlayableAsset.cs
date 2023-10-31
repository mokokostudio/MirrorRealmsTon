using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class RunModePlayableAsset : PlayableAsset, ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;
        public string mode;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}