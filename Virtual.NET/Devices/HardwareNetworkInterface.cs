using System;
using SharpPcap;
using Virtual.NET.Protocols;

namespace Virtual.NET.Devices {
    public class HardwareNetworkInterface : NetworkDeviceBase {
        private readonly ICaptureDevice _hardwareDevice;

        public HardwareNetworkInterface(ICaptureDevice hardwareDevice) {
            _hardwareDevice = hardwareDevice;

            _hardwareDevice.OnPacketArrival += _onHardwarePacketArrival;
        }

        public override void Start() {
            _hardwareDevice.Open(DeviceMode.Promiscuous);
            _hardwareDevice.StartCapture();

            base.Start();
        }

        public override void Stop() {
            _hardwareDevice.StopCapture();
            _hardwareDevice.Close();

            base.Stop();
        }

        public void SendPacket(LinkLayerProtocolPacket packet) {
            if (!IsRunning) {
                return;
            }

            _hardwareDevice.SendPacket(packet.ToBytes(), packet.Length);
        }

        public void SendPacket(object sender, NetworkDevicePacketEventHandlerArgs args) {
            if (args.NetworkDevice is HardwareNetworkInterface) {
                return;
            }

            if (args.NetworkDevice == this) {
                Console.WriteLine("Loop!");
                return;
            }

            SendPacket(args.Packet);
        }

        private void _onHardwarePacketArrival(object sender, CaptureEventArgs captureEventArgs) {
            if (!IsRunning) {
                return;
            }

            _dispatchOnPacketIncoming(new NetworkDevicePacketEventHandlerArgs(this,
                                                                              LinkLayerProtocolFactory.Create(new RawPacket(captureEventArgs.Packet.Data),
                                                                                                              captureEventArgs.Packet.LinkLayerType)));
        }
    }
}