using System.IO;

namespace Celtic_Guardian.Save_File
{
    public class Save_Data_Chunk
    {
        public virtual void Clear()
        {
        }

        public virtual void Load(byte[] buffer)
        {
            using (var reader = new BinaryReader(new MemoryStream(buffer)))
            {
                Load(reader);
            }
        }

        public virtual void Load(BinaryReader reader)
        {
        }

        public virtual byte[] Save()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                Save(writer);
                return stream.ToArray();
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
        }
    }
}