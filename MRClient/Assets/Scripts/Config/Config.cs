using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using UnityEngine;

public static partial class Config {
    private const string MISSING_LAN = "Missing Language";

    private static string[] s_Lans;

    public class LanReference {
        private int id;

        public static implicit operator LanReference(int id) => new LanReference { id = id };

        public static implicit operator string(LanReference lr) {
            if(lr.id == 0 || lr.id> s_Lans.Length)
                return MISSING_LAN;
            return s_Lans[lr.id - 1];
        }
        public override string ToString() => this;
    }

    public static void LoadLanguage(byte[] data) {
        var loader = new Loader(data);
        var num = loader.ReadInt();
        s_Lans = new string[num];
        for (uint i = 0; i < num; i++) {
            var value = loader.ReadString();
            s_Lans[i] = value;
        }
    }

    public static Dictionary<U, T> Turn<U, T>(IList<T> list, Func<T, U> key) {
        var dic = new Dictionary<U, T>();
        foreach (var o in list)
            dic[key(o)] = o;
        return new Dictionary<U, T>(dic);
    }

    public static Dictionary<U, V> Turn<U, V, T>(IList<T> list, Func<T, U> key, Func<IList<T>, V> turn) {
        var data = new Dictionary<U, List<T>>();
        foreach (var o in list) {
            var k = key(o);
            if (!data.ContainsKey(k))
                data[k] = new List<T>();
            data[k].Add(o);
        }
        var dic = new Dictionary<U, V>();
        foreach (var kv in data)
            dic[kv.Key] = turn(kv.Value);
        return new Dictionary<U, V>(dic);
    }

    internal class Loader {
        private MemoryStream m_Stream;
        private BinaryReader m_Reader;

        public Loader(byte[] data) {
            m_Stream = new MemoryStream(data);
            m_Reader = new BinaryReader(m_Stream);
        }

        public int ReadInt() => m_Reader.ReadInt32();
        public ulong ReadULong() => m_Reader.ReadUInt64();
        public ushort ReadUShort() => m_Reader.ReadUInt16();
        public string ReadString() => m_Reader.ReadString();
        public List<T> ReadArray<T>(Func<T> creater) {
            var len = ReadUShort();
            var result = new T[len];
            for (ushort i = 0; i < len; i++)
                result[i] = creater();
            return new List<T>(result);
        }
        public void Dispose() {
            m_Reader.Dispose();
            m_Stream.Dispose();
        }
    }
}