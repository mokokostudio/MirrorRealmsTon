using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors {

    public abstract class GameEditorWindow : OdinEditorWindow {
        public static GameEditorWindow Current;
        [SerializeField]
        private ItemAssetHandle m_Manager;
        private bool m_IsInitialized;
        [SerializeField]
        private int[] m_windowData;

        protected abstract Type AssetType { get; }

        public void Select(UnityEngine.Object asset) => m_Manager.WeekSelect(asset);
        public void TrySelectMenu() => m_Manager.TrySelectMenu(null);

        protected ItemAssetHandle Manager {
            get {
                if (!m_IsInitialized) {
                    minSize = new Vector2(600, 200);
                    if (!typeof(GameEditorParentAsset).IsAssignableFrom(AssetType)) {
                        Debug.LogError($"Asset of Type: <{GetType()}> is not inherit from <GameEditorParentAsset>");
                        return null;
                    }
                    var config = AssetType.GetAttribute<AssetConfigAttribute>();
                    m_IsInitialized = true;
                    m_Manager = ItemAssetHandle.CreateHandle(AssetType);
                    titleContent.image = GameEditorGUI.GetIcon(config.Icon);
                    titleContent.text = config.Name + "Editor";
                    m_windowData = new int[10];
                    OnInit();
                }
                return m_Manager;
            }
        }

        protected override IEnumerable<object> GetTargets() {
            var target = m_Manager.Selected;
            if (target == null)
                yield break;
            yield return target;
        }

        protected override void OnGUI() {
            Current = this;
            Manager.BeginDraw(m_windowData);
            base.OnGUI();
            Manager.EndDraw();
            Current = null;
        }

        protected override void OnDestroy() {
            m_Manager.Destroy();
        }

        protected virtual void OnInit() { }
    }
}
