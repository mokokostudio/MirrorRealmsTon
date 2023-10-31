using System;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;
using System.Collections.Generic;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(ImpulsePlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class ImpulseTrack : TrackAsset {
        public ImpulseConfig[] Export() {
            var result = new List<ImpulseConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as ImpulsePlayableAsset;
                var config = new ImpulseConfig();
                config.startPoint = clip.start;
                config.onlySelf = asset.onlySelf;
                config.power = asset.power;
                result.Add(config);
            }
            return result.ToArray();
        }
    }
}