using System;
using TrueSync;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(LimitDodgePlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class LimitDodgeTrack : TrackAsset {
        public FP Export() {
            foreach (var clip in GetClips())
                return clip.start;
            return 0;
        }
    }
}