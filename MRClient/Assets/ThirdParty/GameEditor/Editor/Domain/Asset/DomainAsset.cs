using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors.Domain {

    public class DomainAsset : SerializedScriptableObject {
        [LabelText("行为列表")]
        public List<EAPIAct> actions = new List<EAPIAct>();
    }
}
