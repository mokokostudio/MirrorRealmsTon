using System;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;
using System.Collections.Generic;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(RunModePlayableAsset))]
    [TrackColor(1f, .5f, .5f)]
    public class RunModeTrack : TrackAsset {
        public RunModeConfig[] Export() {
            var result = new List<RunModeConfig>();
            foreach (var clip in GetClips()) {
                var p = new RunModeConfig();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                p.mode = (clip.asset as RunModePlayableAsset).mode;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}