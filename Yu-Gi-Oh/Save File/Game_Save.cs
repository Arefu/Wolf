using System.Diagnostics;
using System.IO;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public partial class Game_Save
    {
        private const uint HeaderMagic1 = 0x54CE29F9;
        private const uint HeaderMagic2 = 0x04714D02;

        /// <summary>
        ///     Lengh of SaveFile in Bytes.
        /// </summary>
        public const int FileLength = 29005;

        /// <summary>
        ///     Where SaveStats Information Is Stored.
        /// </summary>
        public const int StatsOffset = 36;

        /// <summary>
        ///     Where BattlePack Information Is Stored.
        /// </summary>
        public const int BattlePacksOffset = 380;

        /// <summary>
        ///     Where Misc Information Is Stored.
        /// </summary>
        public const int MiscDataOffset = 3600;

        /// <summary>
        ///     Where Campaingn Duel Progress Is Stored.
        /// </summary>
        public const int CampaignDataOffset = 5648;

        /// <summary>
        ///     Where User Decks Are Stored.
        /// </summary>
        public const int DecksOffset = 11696;

        /// <summary>
        ///     Where Cards Unlocked / Copies of Cards is stored.
        /// </summary>
        public const int CardListOffset = 21424;

        public Game_Save()
        {
            Clear();
        }

        /// <summary>
        ///     Size of StatSize in bytes represented in the save file
        /// </summary>
        public static int StatsSize => BattlePacksOffset - StatsOffset;

        /// <summary>
        ///     Size of BattlePacks in bytes represented in the save file
        /// </summary>
        public static int BattlePacksSize => MiscDataOffset - BattlePacksOffset;

        /// <summary>
        ///     Size of MiscData in bytes represented in the save file
        /// </summary>
        public static int MiscDataSize => CampaignDataOffset - MiscDataOffset;

        /// <summary>
        ///     Size of CampaignData in bytes represented in the save file
        /// </summary>
        public static int CampaignDataSize => DecksOffset - CampaignDataOffset;

        /// <summary>
        ///     Size of Decks in bytes represented in the save file
        /// </summary>
        public static int DecksSize => CardListOffset - DecksOffset;

        /// <summary>
        ///     Size of CardList in bytes represented in the save file
        /// </summary>
        public static int CardListSize => FileLength - CardListOffset;

        /// <summary>
        ///     How many times have you played the game
        /// </summary>
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

            if (BattlePacks == null || BattlePacks.Length != Constants.NumBattlePacks)
                BattlePacks = new Battle_Pack_Save[Constants.NumBattlePacks];

            for (var Counter = 0; Counter < Constants.NumBattlePacks; Counter++)
            {
                if (BattlePacks[Counter] == null) BattlePacks[Counter] = new Battle_Pack_Save();

                BattlePacks[Counter].Clear();
            }

            if (Misc == null) Misc = new Misc_Save();

            Misc.Clear();

            if (Campaign == null) Campaign = new Campaign_Save();

            Campaign.Clear();

            if (Decks == null || Decks.Length != Constants.NumUserDecks) Decks = new Deck_Save[Constants.NumUserDecks];

            for (var Counter = 0; Counter < Constants.NumUserDecks; Counter++)
            {
                if (Decks[Counter] == null) Decks[Counter] = new Deck_Save();

                Decks[Counter].Clear();
            }

            if (CardList == null) CardList = new Card_List_Save();

            CardList.Clear();
        }

        public bool Load()
        {
            return Load(GetSaveFilePath());
        }

        public bool Load(string Path)
        {
            if (string.IsNullOrEmpty(Path) || !File.Exists(Path)) return false;

            return Load(File.ReadAllBytes(Path), true);
        }

        public bool Load(byte[] Buffer)
        {
            return Load(Buffer, false);
        }

        private bool Load(byte[] Buffer, bool CheckSignature)
        {
            Clear();

            try
            {
                using (var Reader = new BinaryReader(new MemoryStream(Buffer)))
                {
                    var Signature = Reader.ReadUInt32();
                    if (CheckSignature)
                    {
                        var CalculatedSignature = GetSignature(Buffer);
                        Debug.Assert(CalculatedSignature == Signature, "Bad save data signature");
                    }

                    PlayCount = Reader.ReadInt32();

                    Stats.Load(Reader);

                    for (var Counter = 0; Counter < Constants.NumBattlePacks; Counter++)
                        BattlePacks[Counter].Load(Reader);

                    Misc.Load(Reader);

                    Campaign.Load(Reader);

                    for (var Counter = 0; Counter < Constants.NumUserDecks; Counter++) Decks[Counter].Load(Reader);

                    CardList.Load(Reader);
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

        public void Save(string Path)
        {
            var Buffer = ToArray();
            if (Buffer != null) File.WriteAllBytes(Path, Buffer);
        }

        public byte[] ToArray()
        {
            using (var Stream = new MemoryStream())
            using (var Writer = new BinaryWriter(Stream))
            {
                Writer.Write(HeaderMagic1);
                Writer.Write(HeaderMagic2);
                Writer.Write(FileLength);
                Writer.Write((uint) 0);
                Writer.Write(PlayCount);

                Writer.Write((uint) 5);
                Writer.Write((uint) 5);
                Writer.Write((uint) 0);
                Writer.Write((uint) 0x3F800000);

                (Stats ?? new Stat_Save()).Save(Writer);

                for (var Counter = 0; Counter < Constants.NumBattlePacks; Counter++)
                    (BattlePacks[Counter] ?? new Battle_Pack_Save()).Save(Writer);

                (Misc ?? new Misc_Save()).Save(Writer);

                (Campaign ?? new Campaign_Save()).Save(Writer);

                for (var Counter = 0; Counter < Constants.NumUserDecks; Counter++)
                    (Decks[Counter] ?? new Deck_Save()).Save(Writer);

                (CardList ?? new Card_List_Save()).Save(Writer);

                var Buffer = Stream.ToArray();
                SaveSignature(Buffer);
                return Buffer;
            }
        }
    }
}