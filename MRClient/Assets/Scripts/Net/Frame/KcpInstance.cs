using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace MR.Net.Frame {
    public class KcpInstance {
        private KcpService m_Kcp;
        private KcpService.Client m_KcpClient;
        private Queue<byte[]> m_Datas = new Queue<byte[]>();
        private Action<byte[]> m_OnCall;

        internal KcpInstance(string ip, int portUdp) {
            var success = false;
            var port = 40000;
            while (!success) {
                try {
                    m_Kcp = new KcpService(port++);
                    success = true;
                } catch { }
            }
            m_KcpClient = m_Kcp.GetClient(new IPEndPoint(IPAddress.Parse(ip), portUdp));
            Read();
        }

        private async void Read() {
            while (m_Kcp != null) {
                var data = await m_KcpClient.ReceiveAsync();
                //if (data.Length > 6)
                //    Debug.Log("Recv:" + string.Join(",", data));
                m_Datas.Enqueue(data);
            }
        }

        public void Regist(Action<byte[]> call) => m_OnCall += call;

        public void Send(byte[] data) => m_KcpClient.Send(data, data.Length);

        public void Update() {
            while (m_Datas.TryDequeue(out var data)) {
                try {
                    m_OnCall(data);
                } catch { }
            }
        }

        public void Close() {
            m_Kcp.Dispose();
            m_Kcp = null;
        }
    }
}
