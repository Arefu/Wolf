using System.IO;
using System.Text;
using Celtic_Guardian.File_Handling.Utility;

namespace Celtic_Guardian.File_Handling.Miscellaneous_Files
{
    public class Credits : File_Data
    {
        public readonly Encoding CreditsEncoding = Encoding.Unicode;
        public string Text { get; set; }

        public override void Load(BinaryReader reader, long length)
        {
            Text = CreditsEncoding.GetString(reader.ReadBytes(length));
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(CreditsEncoding.GetBytes(Text ?? string.Empty));
        }
    }
}