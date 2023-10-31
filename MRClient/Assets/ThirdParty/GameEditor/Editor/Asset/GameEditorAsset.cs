using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace GameEditor.Editors {
    public abstract class GameEditorAsset : SerializedScriptableObject {
        public AssetConfigAttribute AssetConfig => this.GetAttribute<AssetConfigAttribute>();
        public NumberRangeAttribute NumberConfig => this.GetAttribute<NumberRangeAttribute>();
        public IDRuleAttribute IDRuleConfig => this.GetAttribute<IDRuleAttribute>();


        public GUIContent Title {
            get {
                var text = GetType().BaseType.IsGenericType ? "Group " : "";
                text += string.IsNullOrEmpty(assetName) ? "Unnamed" : assetName;
                var image = EditorIcons.Transparent.Inactive;
                var attr = GetType().GetAttribute<AssetConfigAttribute>();
                if (attr != null)
                    image = GameEditorGUI.GetIcon(attr.Icon);
                return new GUIContent { text = text, image = image };
            }
        }


        [ShowInInspector]
        [FoldoutGroup("Basic")]
        [LabelText("Number")]
        [DelayedProperty]
        public int AssetNumber {
            get => GetAssetNumber(); set {
                if (value < NumberConfig.Min || value > NumberConfig.Max)
                    return;
                SetAssetNumber(value);
            }
        }
        public string AssetNumberString => AssetNumber.ToString(NumberConfig.Format);
        public override string ToString() {
            if (string.IsNullOrEmpty(assetName))
                return $"{AssetNumberString}\tUnnamed";
            return $"{AssetNumberString}\t{assetName}";
        }
        public abstract void Delete();

        public abstract int GetAssetNumber();
        public abstract void SetAssetNumber(int number);

        [FoldoutGroup("Basic")]
        [LabelText("Name")]
        public string assetName;

        public virtual void OnMenuSelected() { }
    }
}
