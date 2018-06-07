using System;
using System.Collections.Generic;
using System.IO;

namespace Yu_Gi_Oh.Save_File
{
    public class Battle_Pack_Save : Deck_Save_Database
    {
        public const int NumDraftedCards = 90;
        public const int NumSealedCardsPerSegment = 15;

        public Battle_Pack_Save()
        {
            DraftedCards = new List<short>();
            SealedCards1 = new List<short>();
            SealedCards2 = new List<short>();
            SealedCards3 = new List<short>();
        }

        public BattlePackSaveState State { get; set; }

        public int NumDuelsCompleted { get; set; }
        public int NumDuelsWon { get; set; }
        public int NumDuelsLost { get; set; }

        public BattlePackDuelResult DuelResult1 { get; set; }
        public BattlePackDuelResult DuelResult2 { get; set; }
        public BattlePackDuelResult DuelResult3 { get; set; }
        public BattlePackDuelResult DuelResult4 { get; set; }
        public BattlePackDuelResult DuelResult5 { get; set; }

        public List<short> DraftedCards { get; }

        public List<short> SealedCards1 { get; }
        public List<short> SealedCards2 { get; }
        public List<short> SealedCards3 { get; }

        public BattlePackType Type { get; set; }

        public byte Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public int Unk5 { get; set; }
        public byte Unk6 { get; set; }

        public override void Clear()
        {
            base.Clear();

            State = BattlePackSaveState.None;

            NumDuelsCompleted = 0;
            NumDuelsWon = 0;
            NumDuelsLost = 0;

            DuelResult1 = BattlePackDuelResult.None;
            DuelResult2 = BattlePackDuelResult.None;
            DuelResult3 = BattlePackDuelResult.None;
            DuelResult4 = BattlePackDuelResult.None;
            DuelResult5 = BattlePackDuelResult.None;

            DraftedCards.Clear();

            SealedCards1.Clear();
            SealedCards2.Clear();
            SealedCards3.Clear();

            Type = default(BattlePackType);
        }

        public override void Load(BinaryReader reader)
        {
            Unk1 = reader.ReadByte();
            State = (BattlePackSaveState) reader.ReadInt32();
            NumDuelsCompleted = reader.ReadInt32();
            NumDuelsWon = reader.ReadInt32();
            NumDuelsLost = reader.ReadInt32();
            Unk2 = reader.ReadInt32();
            DuelResult1 = (BattlePackDuelResult) reader.ReadInt32();
            DuelResult2 = (BattlePackDuelResult) reader.ReadInt32();
            DuelResult3 = (BattlePackDuelResult) reader.ReadInt32();
            DuelResult4 = (BattlePackDuelResult) reader.ReadInt32();
            DuelResult5 = (BattlePackDuelResult) reader.ReadInt32();

            LoadDeckData(reader);

            Type = (BattlePackType) reader.ReadInt32();

            for (var i = 0; i < NumDraftedCards; i++)
            {
                var cardId = reader.ReadInt16();
                if (cardId > 0) DraftedCards.Add(cardId);
            }

            Unk3 = reader.ReadInt32();
            Unk4 = reader.ReadInt32();
            Unk5 = reader.ReadInt32();
            Unk6 = reader.ReadByte();

            LoadSealedCards(reader, SealedCards1);
            LoadSealedCards(reader, SealedCards2);
            LoadSealedCards(reader, SealedCards3);
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Unk1);
            writer.Write((int) State);
            writer.Write(NumDuelsCompleted);
            writer.Write(NumDuelsWon);
            writer.Write(NumDuelsLost);
            writer.Write(Unk2);
            writer.Write((int) DuelResult1);
            writer.Write((int) DuelResult2);
            writer.Write((int) DuelResult3);
            writer.Write((int) DuelResult4);
            writer.Write((int) DuelResult5);

            SaveDeckData(writer);

            writer.Write((int) Type);

            for (var i = 0; i < NumDraftedCards; i++)
            {
                var cardId = (short) (State == BattlePackSaveState.Created ? -1 : 0);
                if (DraftedCards.Count > i) cardId = DraftedCards[i];
                writer.Write(cardId);
            }

            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            writer.Write(Unk6);

            SaveSealedCards(writer, SealedCards1);
            SaveSealedCards(writer, SealedCards2);
            SaveSealedCards(writer, SealedCards3);
        }

        private static void LoadSealedCards(BinaryReader reader, List<short> sealedCards)
        {
            var numSealedCards = reader.ReadInt32();
            for (var i = 0; i < NumSealedCardsPerSegment; i++)
            {
                var cardId = reader.ReadInt16();
                if (cardId > 0) sealedCards.Add(cardId);
            }
        }

        private static void SaveSealedCards(BinaryWriter writer, IReadOnlyList<short> sealedCards)
        {
            writer.Write(Math.Min(NumSealedCardsPerSegment, sealedCards.Count));
            for (var i = 0; i < NumSealedCardsPerSegment; i++)
            {
                short cardId = 0;
                if (sealedCards.Count > i) cardId = sealedCards[i];
                writer.Write(cardId);
            }
        }
    }
}