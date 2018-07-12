using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public class Deck_Save_Database : Save_Data_Chunk
    {
        private const int MaxMainDeckCards = 60;
        private const int MaxSideDeckCards = 15;
        private const int MaxExtraDeckCards = 15;

        public Deck_Save_Database()
        {
            MainDeckCards = new List<short>();
            SideDeckCards = new List<short>();
            ExtraDeckCards = new List<short>();
        }

        public string DeckName { get; set; }
        public List<short> MainDeckCards { get; }
        public List<short> SideDeckCards { get; }
        public List<short> ExtraDeckCards { get; }

        public int DeckAvatarId { get; set; }
        public bool IsDeckComplete { get; set; }

        public override void Clear()
        {
            ClearDeckData();
        }

        protected void ClearDeckData()
        {
            DeckName = null;
            MainDeckCards.Clear();
            SideDeckCards.Clear();
            ExtraDeckCards.Clear();
            DeckAvatarId = 0;
            IsDeckComplete = false;
        }

        public void LoadDeckData(BinaryReader Reader)
        {
            DeckName = Encoding.Unicode.GetString(Reader.ReadBytes(Constants.DeckNameByteLen)).TrimEnd('\0');

            var NumMainDeckCards = Reader.ReadInt16();
            var NumSideDeckCards = Reader.ReadInt16();
            var NumExtraDeckCards = Reader.ReadInt16();

            for (var Count = 0; Count < MaxMainDeckCards; Count++)
            {
                var CardId = Reader.ReadInt16();
                if (CardId > 0) MainDeckCards.Add(CardId);
            }

            for (var Count = 0; Count < MaxSideDeckCards; Count++)
            {
                var CardId = Reader.ReadInt16();
                if (CardId > 0) SideDeckCards.Add(CardId);
            }

            for (var Count = 0; Count < MaxExtraDeckCards; Count++)
            {
                var CardId = Reader.ReadInt16();
                if (CardId > 0) ExtraDeckCards.Add(CardId);
            }

            Reader.ReadBytes(12);
            Reader.ReadBytes(12);
            Reader.ReadUInt32();
            Reader.ReadUInt32();
            Reader.ReadUInt32();
            DeckAvatarId = Reader.ReadInt32();
            Reader.ReadUInt32();
            Reader.ReadUInt32();
            IsDeckComplete = Reader.ReadUInt32() == 1;
        }

        protected void SaveDeckData(BinaryWriter Writer)
        {
            Writer.Write(Encoding.Unicode.GetBytes(DeckName, Constants.DeckNameByteLen, Constants.DeckNameUsableLen));

            Writer.Write((short) Math.Min(MaxMainDeckCards, MainDeckCards.Count));
            Writer.Write((short) Math.Min(MaxSideDeckCards, SideDeckCards.Count));
            Writer.Write((short) Math.Min(MaxExtraDeckCards, ExtraDeckCards.Count));

            for (var Count = 0; Count < MaxMainDeckCards; Count++)
            {
                short CardId = 0;
                if (MainDeckCards.Count > Count) CardId = MainDeckCards[Count];
                Writer.Write(CardId);
            }

            for (var Count = 0; Count < MaxSideDeckCards; Count++)
            {
                short CardId = 0;
                if (SideDeckCards.Count > Count) CardId = SideDeckCards[Count];
                Writer.Write(CardId);
            }

            for (var Count = 0; Count < MaxExtraDeckCards; Count++)
            {
                short CardId = 0;
                if (ExtraDeckCards.Count > Count) CardId = ExtraDeckCards[Count];
                Writer.Write(CardId);
            }

            Writer.Write(new byte[12]);
            Writer.Write(new byte[12]);
            Writer.Write((uint) 0);
            Writer.Write((uint) 0);
            Writer.Write((uint) 0);
            Writer.Write(DeckAvatarId);
            Writer.Write((uint) 0);
            Writer.Write((uint) 0);
            Writer.Write((uint) (IsDeckComplete ? 1 : 0));
        }
    }
}