using System;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;
using System.Collections.Generic;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(FacePlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class FaceTrack : TrackAsset {
        public FaceConfig[] Export() {
            var result = new List<FaceConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as FacePlayableAsset;
                var config = new FaceConfig();
                config.point = clip.start;
                config.lockTarget = asset.lockTarget;
                config.maxAngle = asset.maxAngle;
                result.Add(config);
            }
            return result.ToArray();
        }
    }
}