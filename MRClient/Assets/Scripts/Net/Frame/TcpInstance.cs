using MR.Net.Proto;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace MR.Net.Frame {
    public class TcpInstance {
        private TcpProtoHandle m_Handle;
        private Queue<Action> m_CallBuffer = new Queue<Action>();
        private Dictionary<Delegate, Delegate> m_ConvertDic = new Dictionary<Delegate, Delegate>();
        internal TcpInstance(string address, int port) {
            var client = new TcpClient(address, port);
            m_Handle = new TcpProtoHandle(client);
        }

        public void Close() => m_Handle.Close();
        public void Regist<T>(Action<T> callback) {
            Action<T> nCall = (T m) => m_CallBuffer.Enqueue(() => callback(m));
            m_ConvertDic[callback] = nCall;
            m_Handle.Regist(nCall);
        }
        public void UnRegist<T>(Action<T> callback) {
            if (!m_ConvertDic.TryGetValue(callback, out var d))
                return;
            Action<T> nCall = d as Action<T>;
            m_Handle.UnRegist(nCall);
        }
        public void RegistOnce<T>(Action<T> callback) => m_Handle.RegistOnce<T>(m => m_CallBuffer.Enqueue(() => callback(m)));
        public void Send<T>(T obj) => m_Handle.Send(obj);
        public void Send<T, U>(T obj, Action<U> callback) => m_Handle.Send<T, U>(obj, m => m_CallBuffer.Enqueue(() => callback(m)));

        public void Update() {
            while (m_CallBuffer.TryDequeue(out var call))
                try { call.Invoke(); } catch (Exception e) { Debug.LogError(e); }
        }
    }
}
