using System;
using UnityEngine.Playables;
using UnityEngine;
using TrueSync;

namespace MR.Battle.Timeline {
    [Serializable]
    public class ImpulsePlayableAsset : PlayableAsset {
        public bool onlySelf;
        public TSVector power;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return Playable.Create(graph);
        }
    }
}