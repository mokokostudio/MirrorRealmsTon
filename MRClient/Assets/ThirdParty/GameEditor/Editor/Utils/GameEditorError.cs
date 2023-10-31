using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors {
    public static class GameEditorError {
        private static List<string> s_Keys = new List<string>();
        private static List<Object> s_Objs = new List<Object>();
        public static void PushKey(string key, Object obj = null) {
            var str = key.Replace("\t", " ");
            if (s_Keys.Count == 0)
                s_Keys.Add(str);
            else
                s_Keys.Add(s_Keys[s_Keys.Count - 1] + " 的 " + str);
            s_Objs.Add(obj);
        }

        public static void PopKey() {
            s_Keys.RemoveAt(s_Keys.Count - 1);
            s_Objs.RemoveAt(s_Objs.Count - 1);
        }

        public static string Exception(string msg, out Object obj, bool Clear = false) {
            obj = null;
            for (int i = s_Objs.Count - 1; i >= 0; i--)
                if (s_Objs[i] != null) {
                    obj = s_Objs[i];
                    break;
                }
            if (s_Keys.Count == 0)
                return msg;
            else {
                var str = s_Keys[s_Keys.Count - 1];
                if (Clear)
                    s_Keys.Clear();
                return str + " 发生异常: " + msg;
            }
        }
    }
}