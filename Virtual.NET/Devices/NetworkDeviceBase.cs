using System;
using Virtual.NET.Protocols;

namespace Virtual.NET.Devices {
    public abstract class NetworkDeviceBase : INetworkDevice {
        public delegate void NetworkDevicePacketEventHandler(object sender, NetworkDevicePacketEventHandlerArgs args);

        public bool IsRunning = false;
        public event NetworkDevicePacketEventHandler OnPacketIncoming;

        public virtual void Start() {
            IsRunning = true;
        }

        public virtual void Stop() {
            IsRunning = false;
        }

        protected virtual void _dispatchOnPacketIncoming(NetworkDevicePacketEventHandlerArgs args) {
            NetworkDevicePacketEventHandler handler = OnPacketIncoming;
            if (handler != null) {
                handler(this, args);
            }
        }

        public class NetworkDevicePacketEventHandlerArgs : EventArgs {
            private readonly INetworkDevice _networkDevice;
            private readonly LinkLayerProtocolPacket _packet;

            public NetworkDevicePacketEventHandlerArgs(INetworkDevice networkDevice, LinkLayerProtocolPacket packet) {
                _networkDevice = networkDevice;
                _packet = packet;
            }

            public INetworkDevice NetworkDevice {
                get { return _networkDevice; }
            }

            public LinkLayerProtocolPacket Packet {
                get { return _packet; }
            }
        }
    }

    public interface INetworkDevice {
        event NetworkDeviceBase.NetworkDevicePacketEventHandler OnPacketIncoming;

        void Start();
        void Stop();
    }
}