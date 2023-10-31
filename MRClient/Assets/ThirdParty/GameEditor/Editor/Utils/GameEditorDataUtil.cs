using GameEditor.Editors;
using GameEditor.Editors.Domain;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {

    public static class GameEditorDataUtil {

        private static Regex s_AssetRegex = new Regex(".*(\\{0:0+\\}).*\\.asset");

        private static Dictionary<string, object> s_AssetsCached = new Dictionary<string, object>();
        static GameEditorDataUtil() {
            UnityEditorEventUtility.OnProjectChanged += OnProjectChanged;
        }

        private static void OnProjectChanged() {
            s_AssetsCached.Clear();
        }

        public static List<T> GetAssets<T>(string path) where T : GameEditorAsset {
            if (!s_AssetsCached.ContainsKey(path)) {
                List<T> result = new List<T>();
                var config = typeof(T).GetAttributeCached<NumberRangeAttribute>();
                var groups = s_AssetRegex.Match(path).Groups;
                var a = groups[1];
                var d = path.Substring(0, a.Index);
                var f = path.Substring(a.Index + a.Length);
                if (Directory.Exists(d)) {
                    if (f.StartsWith("/")) {
                        foreach (var dir in Directory.GetDirectories(d))
                            if (int.TryParse(Path.GetFileName(dir), out var number) && number >= config.Min && number <= config.Max)
                                result.Add(AssetDatabase.LoadAssetAtPath<T>($"{dir}{f}"));
                    } else {
                        Regex fR = new Regex("([0-9]+)" + f);
                        foreach (var file in new DirectoryInfo(d).GetFiles()) {
                            var fm = fR.Match(file.Name);
                            if (fm.Success && fm.Groups[0].Value == file.Name && int.TryParse(fm.Groups[1].Value, out var number) && number >= config.Min && number <= config.Max)
                                result.Add(AssetDatabase.LoadAssetAtPath<T>($"{d}/{file.Name}"));
                        }
                    }
                }
                s_AssetsCached[path] = result;
            }
            return new List<T>(s_AssetsCached[path] as List<T>);
        }

        public static T GetParentAsset<T>(ScriptableObject obj) where T : GameEditorParentAsset {
            string path = AssetDatabase.GetAssetPath(obj);
            var t = typeof(T);
            var ac = t.GetAttributeCached<AssetConfigAttribute>();
            Regex regex = new Regex($"{ac.Path}/[0-9]+/");
            var math = regex.Match(path);
            if (!math.Success)
                return null;
            return AssetDatabase.LoadAssetAtPath<T>(math.Groups[0].Value + "data.asset");
        }

        public static List<T> GetParentAssets<T>() where T : GameEditorParentAsset {
            var t = typeof(T);
            var ac = t.GetAttributeCached<AssetConfigAttribute>();
            var nr = t.GetAttributeCached<NumberRangeAttribute>();
            string path = $"{ac.Path}/{{0:{nr.Format}}}/data.asset";
            return GetAssets<T>(path);
        }

        public static T CreateAsset<T>(string path, int number, T org = null) where T : ScriptableObject {
            path = string.Format(path, number);
            if (File.Exists(path))
                return null;
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (org == null) {
                var data = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(data, path);
                foreach (var name in typeof(T).GetAttributeCached<HasDomainAssetAttribute>().DomainNames)
                    CreateInternalAsset<DomainAsset>(data, name, false);
                AssetDatabase.SaveAssetIfDirty(data);
                return data;
            } else if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(org), path))
                return AssetDatabase.LoadAssetAtPath<T>(path);
            return null;
        }

        public static List<DomainAsset> GetDomainAssets(this GameEditorAsset parent, params string[] names) {
            var oList = GetInternalAssets<DomainAsset>(parent);
            var nList = new List<DomainAsset>();
            for (int i = 0; i < names.Length; i++) {
                var asset = oList.Find(m => m.name == names[i]);
                if (asset != null) {
                    oList.Remove(asset);
                    nList.Add(asset);
                    continue;
                }
                nList.Add(CreateInternalAsset<DomainAsset>(parent, names[i], false));
            }
            bool isDirty = false;
            foreach (var obj in oList) {
                AssetDatabase.RemoveObjectFromAsset(obj);
                EditorUtility.SetDirty(parent);
                isDirty = true;
            }
            if (isDirty)
                AssetDatabase.SaveAssetIfDirty(parent);
            return nList;
        }

        public static List<T> GetInternalAssets<T>(ScriptableObject parent) where T : ScriptableObject {
            List<T> list = new List<T>();
            string path = AssetDatabase.GetAssetPath(parent);
            foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(path)) {
                if (obj == parent || !(obj is T))
                    continue;
                list.Add(obj as T);
            }
            return list;
        }

        public static T CreateInternalAsset<T>(ScriptableObject parent, string name, bool save = true) where T : ScriptableObject {
            string path = AssetDatabase.GetAssetPath(parent);
            var obj = ScriptableObject.CreateInstance<T>();
            obj.name = name;
            AssetDatabase.AddObjectToAsset(obj, path);
            EditorUtility.SetDirty(obj);
            if (save)
                AssetDatabase.SaveAssetIfDirty(parent);
            return obj;
        }

        public static void SetDirty(this InspectorProperty property) => EditorUtility.SetDirty(property.SerializationRoot.ValueEntry.WeakSmartValue as UnityEngine.Object);

        private static Dictionary<Type, Dictionary<Type, Attribute>> s_AttributeDic = new Dictionary<Type, Dictionary<Type, Attribute>>();
        public static T GetAttribute<T>(this GameEditorAsset asset) where T : Attribute, new() => asset.GetType().GetAttributeCached<T>();

        public static T GetAttributeCached<T>(this Type type) where T : Attribute, new() {
            if (!s_AttributeDic.ContainsKey(type))
                s_AttributeDic[type] = new Dictionary<Type, Attribute>();
            var dic = s_AttributeDic[type];
            var aType = typeof(T);
            if (!dic.ContainsKey(aType)) {
                var att = type.GetAttribute<T>();
                if (att == null)
                    att = new T();
                dic[aType] = att;
            }

            return dic[aType] as T;
        }

        public static List<T> LoadAllAssetsAtPath<T>(string path) where T : UnityEngine.Object {
            var list = new List<T>();
            if (Directory.Exists(path))
                LoadAllAssetsAtPath(path, list);
            return list;
        }

        private static void LoadAllAssetsAtPath<T>(string path, List<T> list) where T : UnityEngine.Object {
            foreach (var file in Directory.GetFiles(path)) {
                var obj = AssetDatabase.LoadAssetAtPath<T>(file);
                if (obj != null)
                    list.Add(obj);
            }
            foreach (var dir in Directory.GetDirectories(path))
                LoadAllAssetsAtPath(dir, list);
        }

        public static T LoadOrCreate<T>(string path) where T : ScriptableObject {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null) {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
            }
            return asset;
        }
    }
}
