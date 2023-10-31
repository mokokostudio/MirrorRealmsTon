using System;

namespace GameEditor.Editors {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HasAssetAttribute : Attribute {
        public Type AssetType;
        public HasAssetAttribute(Type type) => AssetType = type;
    }
}
