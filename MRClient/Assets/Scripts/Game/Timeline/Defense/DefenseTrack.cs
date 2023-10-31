using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.VFX;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(DefensePlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class DefenseTrack : TrackAsset {
        public SimpleConfig[] Export() {
            var result = new List<SimpleConfig>();
            foreach (var clip in GetClips()) {
                var p = new SimpleConfig();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}