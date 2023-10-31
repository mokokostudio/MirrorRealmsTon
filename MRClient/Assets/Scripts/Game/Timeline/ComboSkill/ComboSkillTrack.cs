using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(ComboSkillPlayableAsset))]
    [TrackColor(.5f, 1f, .5f)]
    public class ComboSkillTrack : TrackAsset {

        public ComboSkillPoint[] Export() {
            var result = new List<ComboSkillPoint>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as ComboSkillPlayableAsset;
                var p = new ComboSkillPoint();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                p.skill = asset.Skill;
                p.release = asset.ReleaseBtn;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}