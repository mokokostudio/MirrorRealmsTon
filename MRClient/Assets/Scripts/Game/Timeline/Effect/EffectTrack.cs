using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(EffectPlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class EffectTrack : TrackAsset {
        public EffectConfig[] Export() {
            var result = new List<EffectConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as EffectPlayableAsset;
                var p = new EffectConfig();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                p.target = asset.target;
                p.follow = asset.follow;
                p.speed = asset.speed;
                p.position = asset.position;
                p.rotation = asset.rotation;
                p.scale = asset.scale;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}