using System;
using System.Net.NetworkInformation;
using Virtual.NET.Protocols;
using Virtual.NET.Protocols.LinkLayer;

namespace Virtual.NET.Devices {
    public class VirtualNetworkInterface : NetworkDeviceBase {
        protected ILinkLayerProtocolHandler ProtocolHandler;

        public VirtualNetworkInterface(PhysicalAddress physicalAddress) {
            ProtocolHandler = new EthernetProtocolHandler(physicalAddress);
        }

        public event NetworkDevicePacketEventHandler OnPacketOutgoing;

        public void SendPacket(LinkLayerProtocolPacket packet) {
            _dispatchOnPacketOutgoing(new NetworkDevicePacketEventHandlerArgs(this, packet));
        }

        internal void ProcessPacket(object sender, NetworkDevicePacketEventHandlerArgs args) {
            if (!IsRunning) {
                return;
            }

            if (args.NetworkDevice == this) {
                Console.WriteLine("Loop!");
                return;
            }

            _dispatchOnPacketIncoming(args);
        }

        protected void _dispatchOnPacketOutgoing(NetworkDevicePacketEventHandlerArgs args) {
            NetworkDevicePacketEventHandler handler = OnPacketOutgoing;
            if (handler != null) {
                handler(this, args);
            }
        }

        protected override void _dispatchOnPacketIncoming(NetworkDevicePacketEventHandlerArgs args) {
            if (ProtocolHandler.IsValidPacket(args.Packet)) {
                base._dispatchOnPacketIncoming(args);
            }
        }
    }
}