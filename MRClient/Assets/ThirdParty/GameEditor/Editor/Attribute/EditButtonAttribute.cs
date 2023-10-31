using System;

namespace GameEditor.Editors {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditButtonAttribute : Attribute {
        public string asset;
        public EditButtonAttribute() { }
        public EditButtonAttribute(string asset) => this.asset = asset;
    }
}

