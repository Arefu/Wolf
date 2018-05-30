using System.IO;

namespace Celtic_Guardian.Save_File
{
    public class Deck_Save : Deck_Save_Database
    {
        public override void Load(BinaryReader reader)
        {
            LoadDeckData(reader);
        }

        public override void Save(BinaryWriter writer)
        {
            SaveDeckData(writer);
        }
    }
}