using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using System;
using UnityEditor;

namespace GameEditor.Editors
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AssetSelecterAttribute : Attribute
    {
        public string list;
        public bool hasNull = false;
        public bool hasEditor = true;
        public string hasName = "无";

        public AssetSelecterAttribute()
        {
        }

        public AssetSelecterAttribute(string list, bool hasNull = false, bool hasEditor = true, string hasName = "无")
        {
            this.list = list;
            this.hasNull = hasNull;
            this.hasEditor = hasEditor;
            this.hasName = hasName;
        }
    }
}