namespace Virtual.NET.Util {
    public class Endian {
        public static short Swap(short v) {
            return (short) (((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static ushort Swap(ushort v) {
            return (ushort) (((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }

        public static int Swap(int v) {
            return (((Swap((short) v) & 0xffff) << 0x10) |
                    (Swap((short) (v >> 0x10)) & 0xffff));
        }

        public static uint Swap(uint v) {
            return (uint) (((Swap((ushort) v) & 0xffff) << 0x10) |
                           (Swap((ushort) (v >> 0x10)) & 0xffff));
        }

        public static long Swap(long v) {
            return (((Swap((int) v) & 0xffffffffL) << 0x20) |
                    (Swap((int) (v >> 0x20)) & 0xffffffffL));
        }

        public static ulong Swap(ulong v) {
            return (ulong) (((Swap((uint) v) & 0xffffffffL) << 0x20) |
                            (Swap((uint) (v >> 0x20)) & 0xffffffffL));
        }
    }
}