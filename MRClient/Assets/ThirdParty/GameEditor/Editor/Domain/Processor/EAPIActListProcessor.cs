using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;

namespace GameEditor.Editors.Domain {
    public class EAPIActListProcessor : OdinAttributeProcessor<List<EAPIAct>> {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes) {
            attributes.Add(new ListDrawerSettingsAttribute { Expanded = true });
            attributes.Add(new DisableContextMenuAttribute(true, true));
        }
    }
}

