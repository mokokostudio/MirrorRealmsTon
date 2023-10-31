using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;

namespace GameEditor.Editors.Domain {
    public class EAPIActProcessor<T> : OdinAttributeProcessor<T> where T : EAPIAct {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes) {
            attributes.Add(new HideReferenceObjectPickerAttribute());
        }
    }
}

