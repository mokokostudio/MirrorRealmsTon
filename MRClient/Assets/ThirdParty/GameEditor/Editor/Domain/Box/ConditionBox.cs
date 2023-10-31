using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    public class ConditionBox {
        public bool enable;
        [LabelText("当")]
        public EAPIReturn<bool> condition;
    }
}
