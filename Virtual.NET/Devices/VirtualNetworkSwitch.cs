using System;
using System.Collections.Generic;

namespace Virtual.NET.Devices {
    public class VirtualNetworkSwitch : NetworkDeviceBase {
        private readonly List<INetworkDevice> _networkDevices = new List<INetworkDevice>();

        public List<INetworkDevice> NetworkDevices {
            get { return _networkDevices; }
        }

        public void AddDevice(INetworkDevice device) {
            _networkDevices.Add(device);

            if (device is HardwareNetworkInterface) {
                var hardwareDevice = device as HardwareNetworkInterface;

                hardwareDevice.OnPacketIncoming += ProcessPacket;
                OnPacketIncoming += hardwareDevice.SendPacket;
            }
            else if (device is VirtualNetworkInterface) {
                var virtualEthernetNetworkDevice = (VirtualNetworkInterface) device;

                OnPacketIncoming += virtualEthernetNetworkDevice.ProcessPacket;
                virtualEthernetNetworkDevice.OnPacketOutgoing += _dispatchOnPacketIncoming;
            }
            else if (device is VirtualNetworkSwitch) {
                var virtualDevice = (VirtualNetworkSwitch) device;

                OnPacketIncoming += virtualDevice.ProcessPacket;
                virtualDevice.OnPacketIncoming += _dispatchOnPacketIncoming;
            }
        }

        public void RemoveDevice(INetworkDevice device) {
            _networkDevices.Remove(device);

            if (device is HardwareNetworkInterface) {
                var hardwareDevice = device as HardwareNetworkInterface;

                hardwareDevice.OnPacketIncoming -= ProcessPacket;
                OnPacketIncoming -= hardwareDevice.SendPacket;
            }
            else if (device is VirtualNetworkInterface) {
                var virtualEthernetNetworkDevice = (VirtualNetworkInterface) device;

                OnPacketIncoming -= virtualEthernetNetworkDevice.ProcessPacket;
                virtualEthernetNetworkDevice.OnPacketOutgoing -= _dispatchOnPacketIncoming;
            }
            else if (device is VirtualNetworkSwitch) {
                var virtualDevice = (VirtualNetworkSwitch) device;

                OnPacketIncoming -= virtualDevice.ProcessPacket;
                virtualDevice.OnPacketIncoming -= _dispatchOnPacketIncoming;
            }
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

        private void _dispatchOnPacketIncoming(object sender, NetworkDevicePacketEventHandlerArgs args) {
            _dispatchOnPacketIncoming(args);
        }
    }
}