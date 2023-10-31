using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(BulletPlayableAsset))]
    [TrackColor(0, 1, 0)]
    public class BulletTrack : TrackAsset {
        public BulletConfig[] Export() {
            var result = new List<BulletConfig>();
            foreach (var clip in GetClips()) {
                var asset = clip.asset as BulletPlayableAsset;
                var p = new BulletConfig();
                p.startPoint = clip.start;
                p.position = asset.position.ToTSVector();
                p.rotation = asset.rotation.ToTSVector();
                p.atkLv = asset.atkLv;
                p.hitType = asset.hitType;
                p.sp = asset.sp;
                p.hp = asset.hp;
                p.faceTarget = asset.faceTarget;
                p.speed = asset.speed;
                p.trackDeg = asset.trackDeg;
                p.trackTime = asset.trackTime;
                result.Add(p);
            }
            return result.ToArray();
        }
    }
}