using System;
using TrueSync;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    [TrackClipType(typeof(MovePlayableAsset))]
    [TrackColor(.5f, 1f, .5f)]
    public class MoveTrack : TrackAsset {
        public void Export(TSVector2[] data) {
            foreach (var clip in GetClips()) {
                var asset = clip.asset as MovePlayableAsset;
                var start = (int)(clip.start * 60);
                var end = (int)(clip.end * 60);
                var len = end - start;
                var step = asset.Offset / len;
                for (int i = start; i < end; i++) {
                    data[i] = new TSVector2(step.x, step.z);
                }
            }
        }
    }
}