using System;
using System.Runtime.InteropServices;

namespace Virtual.NET {
    public abstract class PacketBase : IPacket {
        public abstract byte[] ToBytes();
    }

    public interface IPacket { }
}