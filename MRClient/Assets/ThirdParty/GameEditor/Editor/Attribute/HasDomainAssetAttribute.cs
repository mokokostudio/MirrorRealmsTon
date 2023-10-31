using System;

namespace GameEditor.Editors {
    [AttributeUsage(AttributeTargets.Class)]
    public class HasDomainAssetAttribute : Attribute {
        public string[] DomainNames;
        public HasDomainAssetAttribute() {
            DomainNames = new string[0];
        }
        public HasDomainAssetAttribute(params string[] domainNames) {
            DomainNames = domainNames;
        }
    }
}
