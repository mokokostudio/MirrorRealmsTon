using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    public class APINameAttribute : Attribute {
        public string name;
        public APINameAttribute(string name) => this.name = name;
    }
}