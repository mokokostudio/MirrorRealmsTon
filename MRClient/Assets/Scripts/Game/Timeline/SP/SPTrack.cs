using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(SPPlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class SPTrack : TrackAsset {
        public SPConfig[] Export() {
            var result = new List<SPConfig>();
            foreach (var clip in GetClips()) {
                var p = new SPConfig();
                p.startPoint = clip.start;
                p.sp = (clip.asset as SPPlayableAsset).value;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}