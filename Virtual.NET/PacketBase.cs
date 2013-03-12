namespace Virtual.NET {
    public abstract class PacketBase : IPacket {
        public abstract int Length { get; }
        public abstract byte[] ToBytes();
    }

    public interface IPacket {
        int Length { get; }
        byte[] ToBytes();
    }
}