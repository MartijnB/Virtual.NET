namespace Virtual.NET {
    public class RawPacket : PacketBase {
        private readonly byte[] _data;

        public RawPacket(byte[] data) {
            _data = data;
        }

        public override int Length {
            get { return _data.Length; }
        }

        public override byte[] ToBytes() {
            return _data;
        }
    }
}