namespace Virtual.NET {
    public interface INetworkDevice {
        event PacketArrivalEventHandler OnPacketArrival;

        void Start();
        void Stop();
    }
}