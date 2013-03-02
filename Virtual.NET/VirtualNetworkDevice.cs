using System;
using SharpPcap;
using Virtual.NET.Protocols;

namespace Virtual.NET {
    public class VirtualNetworkDevice : INetworkDevice {
        public ICaptureDevice HardwareDevice { get; private set; }

        public event PacketArrivalEventHandler OnPacketArrival;

        public VirtualNetworkDevice(ICaptureDevice hardwareDevice) {
            HardwareDevice = hardwareDevice;

            HardwareDevice.OnPacketArrival += _onPacketArrival;
        }

        public void Start() {
            HardwareDevice.Open(DeviceMode.Promiscuous);
            HardwareDevice.StartCapture();
        }

        public void Stop() {
            HardwareDevice.StopCapture();
            HardwareDevice.Close();
        }

        protected void _onPacketArrival(PacketArrivalEventHandlerArgs args) {
            PacketArrivalEventHandler handler = OnPacketArrival;
            if (handler != null) {
                handler(this, args);
            }
        }

        private void _onPacketArrival(object sender, CaptureEventArgs captureEventArgs) {
            _onPacketArrival(new PacketArrivalEventHandlerArgs(this, LinkLayerProtocolFactory.Create(new RawPacket(captureEventArgs.Packet.Data), captureEventArgs.Packet.LinkLayerType)));
        }
    }

    public delegate void PacketArrivalEventHandler(object sender, PacketArrivalEventHandlerArgs args);

    public class PacketArrivalEventHandlerArgs : EventArgs {
        private readonly INetworkDevice _networkDevice;
        private readonly ILinkLayerProtocolPacket _packet;

        public PacketArrivalEventHandlerArgs(INetworkDevice networkDevice, ILinkLayerProtocolPacket packet) {
            _networkDevice = networkDevice;
            _packet = packet;
        }

        public INetworkDevice NetworkDevice {
            get { return _networkDevice; }
        }

        public ILinkLayerProtocolPacket Packet {
            get { return _packet; }
        }
    }
}
