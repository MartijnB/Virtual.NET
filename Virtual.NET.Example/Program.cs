using System;
using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;
using Virtual.NET.Devices;
using Virtual.NET.Protocols.LinkLayer;
using Version = SharpPcap.Version;

namespace Virtual.NET.Example {
    internal class Program {
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

            Console.WriteLine("Starting virtual switch...");

            var virtualSwitch = new VirtualNetworkSwitch();

            /*
            virtualSwitch.OnPacketIncoming += (sender, handlerArgs) => {
                EthernetProtocolPacket packet = handlerArgs.Packet as EthernetProtocolPacket;

                Console.WriteLine("{0} -> {1} Protocol: {2} Length: {3}", packet.SourceAddress, packet.DestinationAddress, packet.PayloadProtocol, packet.PayloadData.Length);
            };
             * */
            

            foreach (ICaptureDevice dev in devices) {
                var hardwareDevice = new HardwareNetworkInterface(dev);
                hardwareDevice.Start();

                virtualSwitch.AddDevice(hardwareDevice);
            }

            virtualSwitch.Start();

            Console.WriteLine("Virtual switch loaded!");

            var virtualEthernetNetworkDevice = new VirtualNetworkInterface(PhysicalAddress.Parse("AB-CD-EF-AB-CD-EF"));
            //virtualEthernetNetworkDevice.AddNetworkDriver(new IPv4NetworkDriver(IPAddress.Parse("10.0.0.240")));

            virtualEthernetNetworkDevice.OnPacketIncoming += (sender, handlerArgs) => {
                EthernetProtocolPacket packet = handlerArgs.Packet as EthernetProtocolPacket;

                Console.WriteLine("{0} -> {1} Protocol: {2} Length: {3}", packet.SourceAddress, packet.DestinationAddress, packet.PayloadProtocol, packet.PayloadData.Length);
            };
            virtualEthernetNetworkDevice.Start();

            virtualSwitch.AddDevice(virtualEthernetNetworkDevice);
        }
    }
}