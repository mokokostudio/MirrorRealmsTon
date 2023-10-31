using MR.Net;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MR.Net.Frame {
    public class Net : MonoBehaviour {
        private static Net Instance;
        public static List<ChannelConfig> Channels => Instance.channels;
        public static int ChannelIdx { get; set; }
        public static ChannelConfig CurrentChannel => Channels[ChannelIdx];

        public List<ChannelConfig> channels;

        private TcpInstance m_TcpInstance;
        private KcpInstance m_KcpInstance;

        private void Awake() {
            Instance = this;
        }

        public static TcpInstance CreateTcp() {
            var channel = CurrentChannel;
            return Instance.m_TcpInstance = new TcpInstance(channel.ip, channel.portTcp);
        }

        public static TcpInstance GetTcp() {
            return Instance.m_TcpInstance;
        }

        public static KcpInstance CreateKcp(int port = 0) {
            var channel = CurrentChannel;
            return Instance.m_KcpInstance = new KcpInstance(channel.ip, port == 0 ? channel.portUdp : port);
        }

        public static KcpInstance GetKcp() {
            return Instance.m_KcpInstance;
        }

        private void Update() {
            m_TcpInstance?.Update();
            m_KcpInstance?.Update();
        }

        [Serializable]
        public class ChannelConfig {
            public string name = "Name";
            public string ip = "127.0.0.1";
            public int portTcp = 12345;
            public int portUdp = 12346;
        }
    }
}
