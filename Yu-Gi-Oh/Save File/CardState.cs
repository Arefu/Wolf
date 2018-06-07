using System.Runtime.InteropServices;

namespace Yu_Gi_Oh.Save_File
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CardState
    {
        public byte RawValue;

        public byte Count
        {
            get => (byte) (RawValue & 7);
            set
            {
                RawValue &= 0xF8;

                RawValue |= (byte) (value & 7);
            }
        }

        public bool Seen
        {
            get => ((RawValue >> 3) & 1) != 0;
            set
            {
                RawValue &= 0xF7;

                if (value) RawValue |= 1 << 3;
            }
        }

        public byte Unkown
        {
            get => (byte) (RawValue >> 4);
            set
            {
                RawValue &= 0xF;
                RawValue |= (byte) ((value & 0xF) << 4);
            }
        }

        public static CardState None => default(CardState);
    }
}