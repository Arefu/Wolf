using Celtic_Guardian.File_Handling.Utility;
using System.IO;
using System.Runtime.InteropServices;

namespace Celtic_Guardian.Save_File
{
    public class Card_List_Save : Save_Data_Chunk
    {
        public CardState[] Cards { get; private set; }

        public Card_List_Save()
        {
            Cards = new CardState[Constants.NumCards];
        }

        public override void Clear()
        {
            for (var i = 0; i < Constants.NumCards; i++)
            {
                Cards[i] = CardState.None;
            }
        }

        public override void Load(BinaryReader reader)
        {
            for (var i = 0; i < Constants.NumCards; i++)
            {
                Cards[i].RawValue = reader.ReadByte();
            }
        }

        public override void Save(BinaryWriter writer)
        {
            for (var i = 0; i < Constants.NumCards; i++)
            {
                writer.Write(Cards[i].RawValue);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CardState
    {
        public byte RawValue;
        public byte Count
        {
            get => (byte)(RawValue & 7);
            set
            {
                RawValue &= 0xF8;
                
                RawValue |= (byte)(value & 7);
            }
        }
        
        public bool Seen
        {
            get => ((RawValue >> 3) & 1) != 0;
            set
            {
                RawValue &= 0xF7;
                
                if (value)
                {
                    RawValue |= (byte)(1 << 3);
                }
            }
        }
        public byte Unkown
        {
            get => (byte)(RawValue >> 4);
            set
            {
                RawValue &= 0xF;
                RawValue |= (byte)((value & 0xF) << 4);
            }
        }
        public static CardState None => default(CardState);
    }
}
