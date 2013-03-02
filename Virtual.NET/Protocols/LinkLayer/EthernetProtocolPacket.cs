using System;
using System.IO;
using System.Net.NetworkInformation;
using Virtual.NET.Protocols.DataLinkLayer;
using Virtual.NET.Util;

namespace Virtual.NET.Protocols.LinkLayer {
    public class EthernetProtocolPacket : PacketBase, ILinkLayerProtocolPacket {
        public PhysicalAddress SourceAddress;
        public PhysicalAddress DestinationAddress;
        public DataLinkLayerProtocol PayloadProtocol;
        public MemoryStream PayloadData;

        public EthernetProtocolPacket() {
        }

        public EthernetProtocolPacket(RawPacket rawPacket) {
            //Console.WriteLine(BitConverter.ToString(rawPacket.ToBytes()).Replace("-", ":"));

            _parsePacket(rawPacket.ToBytes());
        }

        public override byte[] ToBytes() {
            var data = new MemoryStream();

            data.Write(DestinationAddress.GetAddressBytes(), 0, 6);
            data.Write(SourceAddress.GetAddressBytes(), 0, 6);
            data.Write(BitConverter.GetBytes(Endian.Swap((ushort) PayloadProtocol)), 0, 2);
            data.Write(PayloadData.GetBuffer(), 0, (int) PayloadData.Length);

            return data.GetBuffer();
        }

        private void _parsePacket(byte[] data) {
            if (data.Length < 14) {
                throw new ArgumentOutOfRangeException("data", "Invalid packet length!");
            }

            DestinationAddress = new PhysicalAddress(new[] {data[0], data[1], data[2], data[3], data[4], data[5],});
            SourceAddress = new PhysicalAddress(new[] { data[6], data[7], data[8], data[9], data[10], data[11], });
            PayloadProtocol = (DataLinkLayerProtocol) Endian.Swap(BitConverter.ToUInt16(data, 12));
            PayloadData = new MemoryStream(data, 14, data.Length - 14, false, true);
        }
    }
}