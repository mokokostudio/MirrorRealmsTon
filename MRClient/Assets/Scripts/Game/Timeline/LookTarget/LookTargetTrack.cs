using System;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;
using System.Collections.Generic;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(LookTargetPlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class LookTargetTrack : TrackAsset {
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