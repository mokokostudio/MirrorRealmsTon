using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(EndurePlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class EndureTrack : TrackAsset {
        public EndureConfig[] Export() {
            var result = new List<EndureConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as EndurePlayableAsset;
                var p = new EndureConfig();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                p.defLV = asset.defLv;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}