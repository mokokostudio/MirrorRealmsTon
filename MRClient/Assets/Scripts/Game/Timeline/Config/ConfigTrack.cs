using System;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackColor(1f, .5f, .5f)]
    public class ConfigTrack : TrackAsset {
        public bool moveable;
    }
}