using System.Diagnostics;
using System.IO;
using Celtic_Guardian.File_Handling.Utility;

namespace Celtic_Guardian.Save_File
{
    public partial class Game_Save
    {
        private const uint headerMagic1 = 0x54CE29F9;
        private const uint headerMagic2 = 0x04714D02;
        public const int FileLength = 29005;

        public const int UnkOffset1 = 20;
        public const int StatsOffset = 36;
        public const int BattlePacksOffset = 380;
        public const int MiscDataOffset = 3600;
        public const int CampaignDataOffset = 5648;
        public const int DecksOffset = 11696;
        public const int CardListOffset = 21424;

        public Game_Save()
        {
            Clear();
        }

        public static int StatsSize => BattlePacksOffset - StatsOffset;

        public static int BattlePacksSize => MiscDataOffset - BattlePacksOffset;

        public static int MiscDataSize => CampaignDataOffset - MiscDataOffset;

        public static int CampaignDataSize => DecksOffset - CampaignDataOffset;

        public static int DecksSize => CardListOffset - DecksOffset;

        public static int CardListSize => FileLength - CardListOffset;

        public int PlayCount { get; set; }

        public Stat_Save Stats { get; set; }
        public Battle_Pack_Save[] BattlePacks { get; private set; }
        public Misc_Save Misc { get; set; }
        public Campaign_Save Campaign { get; set; }
        public Deck_Save[] Decks { get; private set; }
        public Card_List_Save CardList { get; private set; }

        public void Clear()
        {
            if (Stats == null) Stats = new Stat_Save();

            Stats.Clear();

            if (BattlePacks == null || BattlePacks.Length != Constants.NumBattlePacks) BattlePacks = new Battle_Pack_Save[Constants.NumBattlePacks];

            for (var i = 0; i < Constants.NumBattlePacks; i++)
            {
                if (BattlePacks[i] == null) BattlePacks[i] = new Battle_Pack_Save();

                BattlePacks[i].Clear();
            }

            if (Misc == null) Misc = new Misc_Save();

            Misc.Clear();

            if (Campaign == null) Campaign = new Campaign_Save();

            Campaign.Clear();

            if (Decks == null || Decks.Length != Constants.NumUserDecks) Decks = new Deck_Save[Constants.NumUserDecks];

            for (var i = 0; i < Constants.NumUserDecks; i++)
            {
                if (Decks[i] == null) Decks[i] = new Deck_Save();

                Decks[i].Clear();
            }

            if (CardList == null) CardList = new Card_List_Save();

            CardList.Clear();
        }

        public bool Load()
        {
            return Load(GetSaveFilePath());
        }

        public bool Load(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

            return Load(File.ReadAllBytes(path), true);
        }

        public bool Load(byte[] buffer)
        {
            return Load(buffer, false);
        }

        private bool Load(byte[] buffer, bool checkSignature)
        {
            Clear();

            try
            {
                using (var reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    Debug.Assert(reader.ReadUInt32() == headerMagic1, "Bad header magic");
                    Debug.Assert(reader.ReadUInt32() == headerMagic2, "Bad header magic");

                    var fileLength = reader.ReadUInt32();
                    Debug.Assert(fileLength == buffer.Length, "Bad save data length");
                    Debug.Assert(fileLength == FileLength, "Bad save data length");

                    var signature = reader.ReadUInt32();
                    if (checkSignature)
                    {
                        var calculatedSignature = GetSignature(buffer);
                        Debug.Assert(calculatedSignature == signature, "Bad save data signature");
                    }

                    PlayCount = reader.ReadInt32();

                    var dataMagic1 = reader.ReadUInt32();
                    var dataMagic2 = reader.ReadUInt32();
                    var dataMagic3 = reader.ReadUInt32();
                    var dataMagic4 = reader.ReadUInt32();

                    Stats.Load(reader);

                    for (var i = 0; i < Constants.NumBattlePacks; i++) BattlePacks[i].Load(reader);

                    Misc.Load(reader);

                    Campaign.Load(reader);

                    for (var i = 0; i < Constants.NumUserDecks; i++) Decks[i].Load(reader);

                    CardList.Load(reader);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            Save(GetSaveFilePath());
        }

        public void Save(string path)
        {
            var buffer = ToArray();
            if (buffer != null) File.WriteAllBytes(path, buffer);
        }

        public byte[] ToArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(headerMagic1);
                writer.Write(headerMagic2);
                writer.Write(FileLength);
                writer.Write((uint) 0);
                writer.Write(PlayCount);

                writer.Write((uint) 5);
                writer.Write((uint) 5);
                writer.Write((uint) 0);
                writer.Write((uint) 0x3F800000);

                (Stats ?? new Stat_Save()).Save(writer);

                for (var i = 0; i < Constants.NumBattlePacks; i++) (BattlePacks[i] != null ? BattlePacks[i] : new Battle_Pack_Save()).Save(writer);

                (Misc ?? new Misc_Save()).Save(writer);

                (Campaign ?? new Campaign_Save()).Save(writer);

                for (var i = 0; i < Constants.NumUserDecks; i++) (Decks[i] != null ? Decks[i] : new Deck_Save()).Save(writer);

                (CardList ?? new Card_List_Save()).Save(writer);

                var buffer = stream.ToArray();
                SaveSignature(buffer);
                return buffer;
            }
        }
    }
}