using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Celtic_Guardian.File_Handling.Utility;

namespace Celtic_Guardian.Save_File
{
    public class Deck_Save_Database : Save_Data_Chunk
    {
        private const int maxMainDeckCards = 60;
        private const int maxSideDeckCards = 15;
        private const int maxExtraDeckCards = 15;

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

        protected void LoadDeckData(BinaryReader reader)
        {
            DeckName = Encoding.Unicode.GetString(reader.ReadBytes(Constants.DeckNameByteLen)).TrimEnd('\0');

            var numMainDeckCards = reader.ReadInt16();
            var numSideDeckCards = reader.ReadInt16();
            var numExtraDeckCards = reader.ReadInt16();

            for (var i = 0; i < maxMainDeckCards; i++)
            {
                var cardId = reader.ReadInt16();
                if (cardId > 0) MainDeckCards.Add(cardId);
            }

            for (var i = 0; i < maxSideDeckCards; i++)
            {
                var cardId = reader.ReadInt16();
                if (cardId > 0) SideDeckCards.Add(cardId);
            }

            for (var i = 0; i < maxExtraDeckCards; i++)
            {
                var cardId = reader.ReadInt16();
                if (cardId > 0) ExtraDeckCards.Add(cardId);
            }

            reader.ReadBytes(12);
            reader.ReadBytes(12);
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            DeckAvatarId = reader.ReadInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            IsDeckComplete = reader.ReadUInt32() == 1;
        }

        protected void SaveDeckData(BinaryWriter writer)
        {
            writer.Write(Encoding.Unicode.GetBytes(DeckName, Constants.DeckNameByteLen, Constants.DeckNameUsableLen));

            writer.Write((short) Math.Min(maxMainDeckCards, MainDeckCards.Count));
            writer.Write((short) Math.Min(maxSideDeckCards, SideDeckCards.Count));
            writer.Write((short) Math.Min(maxExtraDeckCards, ExtraDeckCards.Count));

            for (var i = 0; i < maxMainDeckCards; i++)
            {
                short cardId = 0;
                if (MainDeckCards.Count > i) cardId = MainDeckCards[i];
                writer.Write(cardId);
            }

            for (var i = 0; i < maxSideDeckCards; i++)
            {
                short cardId = 0;
                if (SideDeckCards.Count > i) cardId = SideDeckCards[i];
                writer.Write(cardId);
            }

            for (var i = 0; i < maxExtraDeckCards; i++)
            {
                short cardId = 0;
                if (ExtraDeckCards.Count > i) cardId = ExtraDeckCards[i];
                writer.Write(cardId);
            }

            writer.Write(new byte[12]);
            writer.Write(new byte[12]);
            writer.Write((uint) 0);
            writer.Write((uint) 0);
            writer.Write((uint) 0);
            writer.Write(DeckAvatarId);
            writer.Write((uint) 0);
            writer.Write((uint) 0);
            writer.Write((uint) (IsDeckComplete ? 1 : 0));
        }
    }
}