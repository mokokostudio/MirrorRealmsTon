using System;
using TrueSync;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(EquipPlayableAsset))]
    [TrackColor(.5f, .5f, .5f)]
    public class EquipTrack : TrackAsset {

        public FP Export() {
            foreach (var clip in GetClips())
                return clip.start;
            return -1;
        }
    }
}