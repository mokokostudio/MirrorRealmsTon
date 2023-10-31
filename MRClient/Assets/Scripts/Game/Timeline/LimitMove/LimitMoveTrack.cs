using System;
using TrueSync;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(LimitMovePlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class LimitMoveTrack : TrackAsset {
        public FP Export() {
            foreach (var clip in GetClips())
                return clip.start;
            return 0;
        }
    }
}