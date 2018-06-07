using System.IO;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public class Card_List_Save : Save_Data_Chunk
    {
        public Card_List_Save()
        {
            Cards = new CardState[Constants.NumCards];
        }

        public CardState[] Cards { get; }

        public override void Clear()
        {
            for (var i = 0; i < Constants.NumCards; i++) Cards[i] = CardState.None;
        }

        public override void Load(BinaryReader reader)
        {
            for (var i = 0; i < Constants.NumCards; i++) Cards[i].RawValue = reader.ReadByte();
        }

        public override void Save(BinaryWriter writer)
        {
            for (var i = 0; i < Constants.NumCards; i++) writer.Write(Cards[i].RawValue);
        }
    }
}