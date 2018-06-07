using System.Collections.Generic;
using System.Linq;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.LOTD_Files;
using Yu_Gi_Oh.File_Handling.Main_Files;
using Yu_Gi_Oh.File_Handling.Pack_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Miscellaneous_Files
{
    public class Manager
    {
        public Manager()
        {
            Archive = new LOTD_Archive();
            CurrentLanguage = Localized_Text.Language.English;

            BattlePackData = new List<Battle_Pack>();
            ShopPackData = new List<Shop_Pack>();
        }

        public LOTD_Archive Archive { get; }
        public Deck_Data DeckData { get; set; }
        public Char_Data CharData { get; set; }
        public SKU_Data SkuData { get; set; }
        public Arena_Data ArenaData { get; set; }
        public Duel_Data DuelData { get; set; }
        public Pack_Def_Data PackDefData { get; set; }
        public List<Battle_Pack> BattlePackData { get; }
        public List<Shop_Pack> ShopPackData { get; set; }

        public Card_Limits CardLimits { get; set; }
        public Card_Manager CardManager { get; set; }

        public Localized_Text.Language CurrentLanguage { get; set; }

        public void Load()
        {
            Archive.Load();

            BattlePackData.Clear();
            ShopPackData.Clear();

            DeckData = Archive.LoadLocalizedFile<Deck_Data>();
            CharData = Archive.LoadLocalizedFile<Char_Data>();
            SkuData = Archive.LoadLocalizedFile<SKU_Data>();
            ArenaData = Archive.LoadLocalizedFile<Arena_Data>();
            DuelData = Archive.LoadLocalizedFile<Duel_Data>();
            PackDefData = Archive.LoadLocalizedFile<Pack_Def_Data>();
            CardLimits = Archive.LoadFiles<Card_Limits>()[0];
            BattlePackData.AddRange(Archive.LoadFiles<Battle_Pack>().ToList());
            ShopPackData.AddRange(Archive.LoadFiles<Shop_Pack>().ToList());

            CardManager = new Card_Manager(this);
            CardManager.Load();
        }
    }
}