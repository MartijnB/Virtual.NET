using System;
using SharpPcap;
using Virtual.NET.Protocols.LinkLayer;
using Version = SharpPcap.Version;

namespace Virtual.NET.Example {
    internal class Program {
        private static readonly int _preselectedDevice = 3;

        private static void Main(string[] args) {
            Console.WriteLine("Virtual.NET {0}\n", VirtualNet.Version);
            Console.WriteLine("Using SharpPcap {0}\n", Version.VersionString);

            CaptureDeviceList devices = CaptureDeviceList.Instance;

            if (devices.Count < 1) {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine("The following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            int i = 0;
            foreach (ICaptureDevice dev in devices) {
                Console.WriteLine("{0}) {1}", i, dev.Description);
                i++;
            }

            ICaptureDevice device;
            if (_preselectedDevice == -1) {
                Console.WriteLine();
                Console.Write("-- Please choose a device to send a packet on: ");
                i = int.Parse(Console.ReadLine());

                device = devices[i];
            }
            else {
                device = devices[_preselectedDevice];
            }

            Console.WriteLine();
            Console.WriteLine("Selected device: {0}", device.Description);

            INetworkDevice virtualNetworkDevice = new VirtualNetworkDevice(device);

            virtualNetworkDevice.OnPacketArrival += delegate(object sender, PacketArrivalEventHandlerArgs handlerArgs) {
                EthernetProtocolPacket packet = handlerArgs.Packet as EthernetProtocolPacket;

                //Console.WriteLine(BitConverter.ToString(packet.ToBytes()).Replace("-", ":"));

                Console.WriteLine("{0} -> {1} Protocol: {2} Length: {3}", packet.SourceAddress, packet.DestinationAddress, packet.PayloadProtocol, packet.PayloadData.Length);
            };

            //virtualNetworkDevice.RegisterProtocolHandler(new EthernetProtocolHandler(PhysicalAddress.Parse("AB:CD:EF:12:34:56")));

            virtualNetworkDevice.Start();
        }
    }
}