using System.Net.NetworkInformation;

namespace Virtual.NET.Protocols.LinkLayer {
    public class EthernetProtocolHandler : ILinkLayerProtocolHandler {
        public EthernetProtocolHandler(PhysicalAddress physicalAddress) {
            PhysicalAddress = physicalAddress;
        }

        public PhysicalAddress PhysicalAddress { get; private set; }

        public bool IsValidPacket(LinkLayerProtocolPacket packet) {
            if (!(packet is EthernetProtocolPacket)) {
                return false;
            }

            var ethernetPacket = (EthernetProtocolPacket) packet;

            if (!ethernetPacket.DestinationAddress.Equals(PhysicalAddress) &&
                !ethernetPacket.DestinationAddress.Equals(new PhysicalAddress(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}))) {
                return false;
            }

            return true;
        }
    }
}