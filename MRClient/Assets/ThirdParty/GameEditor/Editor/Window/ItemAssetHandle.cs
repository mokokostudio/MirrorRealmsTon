using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {

    [Serializable]
    public abstract partial class ItemAssetHandle {

        public static ItemAssetHandle CreateHandle(Type type, UnityEngine.Object parentAsset = null) {
            if (typeof(GameEditorParentAsset).IsAssignableFrom(type)) {
                Type hType = typeof(ParentAssetHandle<>).MakeGenericType(type);
                return Activator.CreateInstance(hType) as ItemAssetHandle;
            }
            if (typeof(GameEditorChildAsset).IsAssignableFrom(type)) {
                Type hType = typeof(ChildAssetHandle<>).MakeGenericType(type);
                return Activator.CreateInstance(hType, Path.GetDirectoryName(AssetDatabase.GetAssetPath(parentAsset))) as ItemAssetHandle;
            }
            if (typeof(GameEditorCustomAsset).IsAssignableFrom(type)) {
                Type parent = typeof(ItemAssetCustomHandle<>).MakeGenericType(type);
                foreach (var et in Assembly.GetAssembly(type).GetExportedTypes())
                    if (parent.IsAssignableFrom(et))
                        return Activator.CreateInstance(et, new object[] { parentAsset }) as ItemAssetHandle;
            }
            if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(GameEditorGroupAsset<>)) {
                Type cType = type.BaseType.GetGenericArguments()[0];
                Type hType = typeof(GroupAssetHandle<,>).MakeGenericType(type, cType);
                return Activator.CreateInstance(hType, Path.GetDirectoryName(AssetDatabase.GetAssetPath(parentAsset))) as ItemAssetHandle;
            }
            return null;
        }

        private bool m_IsDirty;
        [SerializeField]
        protected UnityEngine.Object m_CurrentAseet;

        public UnityEngine.Object CurrentAseet => m_CurrentAseet;

        public abstract UnityEngine.Object Selected { get; }
        public ItemAssetHandle() {
            UnityEditorEventUtility.OnProjectChanged += OnProjectChanged;
        }

        public void BeginDraw(int[] data) {
            if (m_IsDirty) {
                ReBuild();
                m_IsDirty = false;
            }
            OnBeginDraw(data);
        }

        public void EndDraw() {
            OnEndDraw();
        }

        public virtual void OnBeginDraw(int[] data) { }
        public virtual void OnEndDraw() { }
        public virtual void ReBuild() { }
        public void Destroy() => UnityEditorEventUtility.OnProjectChanged -= OnProjectChanged;
        protected virtual void OnProjectChanged() => m_IsDirty = true;
        public virtual void Select(UnityEngine.Object asset) {
            if (m_CurrentAseet == asset)
                return;
            m_CurrentAseet = asset;
            if (m_CurrentAseet == null)
                return;
            (asset as GameEditorAsset).OnMenuSelected();
            OnSelected();
        }

        public virtual void WeekSelect(UnityEngine.Object asset) {
            (asset as GameEditorAsset)?.OnMenuSelected();
        }

        public virtual void OnSelected() { }
        public abstract bool TrySelectMenu(UnityEngine.Object asset);

        [Serializable]
        private abstract class BaseAssetHandle<T> : ItemAssetHandle where T : GameEditorAsset {
            protected static UnityEngine.Object s_CopyAsset;

            [SerializeField]
            protected string m_AssetPath;
            [SerializeField]
            private NumberRangeAttribute m_NumberConfig;
            public BaseAssetHandle() {
                var assetConfig = typeof(T).GetAttributeCached<AssetConfigAttribute>();
                m_NumberConfig = typeof(T).GetAttributeCached<NumberRangeAttribute>();
                m_AssetPath = $"{assetConfig.Path}/{{0:{m_NumberConfig.Format}}}";
            }

            public void Create() {
                for (int i = m_NumberConfig.Min; i <= m_NumberConfig.Max; i++) {
                    var item = GameEditorDataUtil.CreateAsset<T>(m_AssetPath, i);
                    if (item == null)
                        continue;
                    Select(item);
                    return;
                }
            }

            public void Copy() {
                s_CopyAsset = m_CurrentAseet;
            }

            public bool CanPaste => (s_CopyAsset as T) != null;

            public void Paste() {
                if (!CanPaste)
                    return;
                for (int i = m_NumberConfig.Min; i <= m_NumberConfig.Max; i++) {
                    var item = GameEditorDataUtil.CreateAsset(m_AssetPath, i, s_CopyAsset as T);
                    if (item == null)
                        continue;
                    Select(item);
                    return;
                }
            }

            public void Delete() {
                GameEditorGUI.ShowConfirm($"确定要删除 {m_CurrentAseet} 吗？", () => {
                    if (m_CurrentAseet is GameEditorAsset)
                        (m_CurrentAseet as GameEditorAsset).Delete();
                    else
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(m_CurrentAseet));
                    Select(null);
                });
            }
        }

    }
}
