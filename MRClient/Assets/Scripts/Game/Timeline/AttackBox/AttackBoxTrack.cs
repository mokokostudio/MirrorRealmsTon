using System;
using System.Collections.Generic;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(AttackBoxPlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class AttackBoxTrack : TrackAsset {
        public AttackBoxConfig[] Export() {
            var result = new List<AttackBoxConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as AttackBoxPlayableAsset;
                var p = new AttackBoxConfig();
                p.startPoint = clip.start;
                p.endPoint = clip.end;
                p.position = asset.position.ToTSVector();
                p.rotation = asset.rotation.ToTSVector();
                p.scale = asset.scale.ToTSVector();
                p.atkLv = asset.atkLv;
                p.hitType = asset.hitType;
                p.sp = asset.sp;
                p.hp = asset.hp;
                p.paush = asset.paush;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}