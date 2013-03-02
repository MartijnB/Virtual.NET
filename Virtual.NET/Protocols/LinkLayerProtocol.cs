using System;
using PacketDotNet;
using Virtual.NET.Protocols.LinkLayer;

namespace Virtual.NET.Protocols {
    public enum LinkLayerProtocol {
        Ethernet
    }

    public class LinkLayerProtocolFactory {
        public static ILinkLayerProtocolPacket Create(RawPacket packet, LinkLayerProtocol protocol) {
            switch (protocol) {
                case LinkLayerProtocol.Ethernet:
                    return new EthernetProtocolPacket(packet);

                default:
                    throw new NotImplementedException("linkLayerType");
            }
        }

        public static ILinkLayerProtocolPacket Create(RawPacket packet, LinkLayers linkLayerType) {
            switch (linkLayerType) {
                case LinkLayers.Ethernet:
                    return new EthernetProtocolPacket(packet);

                default:
                    throw new NotImplementedException("linkLayerType");
            }
        }
    }
}
