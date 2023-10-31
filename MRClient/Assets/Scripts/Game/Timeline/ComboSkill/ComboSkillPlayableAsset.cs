using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;

namespace MR.Battle.Timeline {
    [Serializable]
    public class ComboSkillPlayableAsset : PlayableAsset,ITimelineClipAsset {
        public ClipCaps clipCaps => ClipCaps.None;
        public string Skill;
        public bool ReleaseBtn;


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}