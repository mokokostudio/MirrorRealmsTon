using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors {
    public abstract class ItemAssetCustomHandle<T> : ItemAssetHandle where T : GameEditorCustomAsset {
        public override bool TrySelectMenu(Object asset) => (asset is T) && TrySelectMenu(asset as T);
        public abstract bool TrySelectMenu(T asset);
        protected GameEditorParentAsset m_Parent;
        public ItemAssetCustomHandle() { }
        public ItemAssetCustomHandle(GameEditorParentAsset parent) => m_Parent = parent;
    }
}
