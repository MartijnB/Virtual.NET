using System.Net.NetworkInformation;

namespace Virtual.NET.Protocols.LinkLayer {
    public class EthernetProtocolHandler {
        public PhysicalAddress PhysicalAddress { get; private set; }

        public EthernetProtocolHandler(PhysicalAddress physicalAddress) {
            PhysicalAddress = physicalAddress;
        }
    }
}
