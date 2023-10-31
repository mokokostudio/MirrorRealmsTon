using System;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [APIName("变量")]
    public class EVariablePicker<T> : EAPIReturn<T> {
        public EVariable<T> variable;
        public override string ToString() => "变量" + GetName(variable);
        public override void Check() {
            if (variable == null) 
                throw new Exception("变量未选择");
        }
    }
}
