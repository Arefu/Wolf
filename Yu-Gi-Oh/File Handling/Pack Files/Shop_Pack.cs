using System.IO;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;

namespace Yu_Gi_Oh.File_Handling.Pack_Files
{
    public class Shop_Pack : File_Data
    {
        public Shop_Pack()
        {
            CommonCards = new CardCollection();
            RareCards = new CardCollection();
        }

        public CardCollection CommonCards { get; set; }
        public CardCollection RareCards { get; set; }

        public override void Load(BinaryReader reader, long length)
        {
            CommonCards.Clear();
            RareCards.Clear();

            var commonCardCount = reader.ReadInt16();
            var rareCardCount = reader.ReadInt16();

            for (var i = 0; i < commonCardCount; i++) CommonCards.Add(reader.ReadInt16());

            for (var i = 0; i < rareCardCount; i++) RareCards.Add(reader.ReadInt16());
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write((short) (CommonCards?.CardIds.Count ?? 0));
            writer.Write((short) (RareCards?.CardIds.Count ?? 0));

            if (CommonCards != null)
            {
                CommonCards.Sort();
                foreach (var cardId in CommonCards.CardIds) writer.Write(cardId);
            }

            if (RareCards != null)
            {
                RareCards.Sort();
                foreach (var cardId in RareCards.CardIds) writer.Write(cardId);
            }
        }
    }
}