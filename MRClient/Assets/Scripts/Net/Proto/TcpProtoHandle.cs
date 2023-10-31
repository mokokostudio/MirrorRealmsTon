using ProtoBuf;
using System.Net.Sockets;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;

namespace MR.Net.Proto {

    public class ProtoMessageAttribute : Attribute {
        public int MsgID { get; set; }
        public ProtoMessageAttribute(int msgID) => MsgID = msgID;
    }

    public class TcpProtoHandle {
        private static ConcurrentDictionary<int, Type> s_Msg2TypeDic = new ConcurrentDictionary<int, Type>();
        private static ConcurrentDictionary<Type, int> s_Type2MsgDic = new ConcurrentDictionary<Type, int>();

        private TcpClient m_Client;
        private NetworkStream m_Stream;
        private byte[] m_WriteBuffer = new byte[1024];
        private byte[] m_ReadBuffer = new byte[1024 * 1024];
        private Dictionary<int, Delegate> m_CallBacksP = new Dictionary<int, Delegate>();
        private LinkedList<TempRegistData> m_CallBacksT = new LinkedList<TempRegistData>();
        public event Action onClose;
        private bool m_Closed;

        public TcpProtoHandle(TcpClient client) {
            m_Client = client;
            m_Stream = client.GetStream();
            BeginRead();
        }

        public async void BeginRead() {
            await Task.Delay(10);//为监听留出时间
            try {
                while (!m_Closed)
                    await ReadFromStream();
            } catch {
                Close();
            }
        }

        public void Regist<T>(Action<T> callback) {
            if (!CheckType<T>())
                return;
            var msg = s_Type2MsgDic[typeof(T)];
            if (m_CallBacksP.TryGetValue(msg, out var d))
                m_CallBacksP[msg] = Delegate.Combine(d, callback);
            else
                m_CallBacksP[msg] = callback;
        }

        public void RegistOnce<T>(Action<T> callback) {
            if (!CheckType<T>())
                return;
            var msg = s_Type2MsgDic[typeof(T)];
            m_CallBacksT.AddLast(new TempRegistData { MsgID = msg, Callback = callback });
        }

        public void UnRegist<T>(Action<T> callback) {
            if (!CheckType<T>())
                return;
            var msg = s_Type2MsgDic[typeof(T)];
            if (m_CallBacksP.TryGetValue(msg, out var d))
                m_CallBacksP[msg] = Delegate.Remove(d, callback);
        }

        public void Send<T>(T obj) {
            if (!CheckType<T>())
                return;
            WriteToStream(obj);
        }

        public void Send<T, U>(T obj, Action<U> callback) {
            Send(obj);
            RegistOnce(callback);
        }

        private async Task ReadFromStream() {
            var msg = await ReadNumber();
            var verify = await ReadNumber();
            if (verify != msg.GetHashCode() * 123) {
                Close();
                return;
            }
            var len = await ReadNumber();

            object obj = null;
            var node = m_CallBacksT.First;
            while (node != null) {
                var data = node.Value;
                if (data.MsgID == msg) {
                    obj = await Deserialize(len, msg);
                    m_CallBacksT.Remove(node);
                    AddCall(data.Callback, obj);
                    break;
                }
                node = node.Next;
            }

            if (m_CallBacksP.TryGetValue(msg, out var callback)) {
                if (obj == null)
                    obj = await Deserialize(len, msg);
                AddCall(callback, obj);
                return;
            }

            if (obj == null)
                await ReadSkip(len);
        }

        private async Task<object> Deserialize(int len, int msg) {
            var buffer = len > m_ReadBuffer.Length ? new byte[len] : m_ReadBuffer;
            if (len > 0)
                await m_Stream.ReadAsync(buffer, 0, len);
            using (var ms = new MemoryStream(buffer, 0, len))
                return Serializer.Deserialize(s_Msg2TypeDic[msg], ms);
        }

        private void WriteToStream<T>(T obj) {
            if (m_Closed)
                return;
            var type = typeof(T);
            var msg = s_Type2MsgDic[type];
            var verify = msg.GetHashCode() * 123;
            WriteNumber(msg);
            WriteNumber(verify);
            Serializer.SerializeWithLengthPrefix(m_Stream, obj, PrefixStyle.Fixed32);
            m_Stream.Flush();
        }

        private bool CheckType<T>() {
            var type = typeof(T);
            if (s_Type2MsgDic.ContainsKey(type))
                return true;
            var attr = type.GetCustomAttribute<ProtoMessageAttribute>();
            if (attr == null)
                return false;
            if (s_Msg2TypeDic.ContainsKey(attr.MsgID))
                return false;
            s_Type2MsgDic[type] = attr.MsgID;
            s_Msg2TypeDic[attr.MsgID] = type;
            return true;
        }

        private void AddCall(Delegate callback, object obj) {
            try { callback.DynamicInvoke(obj); } catch { }
        }

        private async Task<int> ReadNumber() {
            var i = await m_Stream.ReadAsync(m_WriteBuffer, 0, 4);
            if (i != 4)
                throw new Exception("Read Data Failed.");
            return BitConverter.ToInt32(m_WriteBuffer, 0);
        }

        private void WriteNumber(int number) {
            m_Stream.Write(BitConverter.GetBytes(number), 0, 4);
        }

        private async Task ReadSkip(int num) {
            while (num > 0) {
                var step = Math.Min(num, m_WriteBuffer.Length);
                int i = await m_Stream.ReadAsync(m_WriteBuffer, 0, step);
                num -= i;
            }
        }

        private class TempRegistData {
            public int MsgID { get; set; }
            public Delegate Callback { get; set; }
        }

        public void Close() {
            m_Closed = true;
            m_Stream.Close();
            m_Client.Close();
            onClose?.Invoke();
        }
    }
}
