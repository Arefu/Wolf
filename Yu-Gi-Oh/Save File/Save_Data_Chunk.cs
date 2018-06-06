using System;
using System.IO;

namespace Yu_Gi_Oh.Save_File
{
    public class Save_Data_Chunk
    {
        /// <summary>
        /// Does Nothing. Don't Use.
        /// </summary>
        [Obsolete]
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Does Nothing. Don't Use.
        /// </summary>
        /// <param name="buffer">Does Nothing. Don't Use.</param>
        [Obsolete]
        public virtual void Load(byte[] buffer)
        {
            using (var reader = new BinaryReader(new MemoryStream(buffer)))
            {
                Load(reader);
            }
        }

        /// <summary>
        /// Does Nothing. Don't Use.
        /// </summary>
        /// <param name="reader">Does Nothing. Don't Use.</param>
        [Obsolete]
        public virtual void Load(BinaryReader reader)
        {
        }

        /// <summary>
        /// Does Nothing. Don't Use.
        /// </summary>
        [Obsolete]
        public virtual byte[] Save()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                Save(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Does Nothing. Don't Use.
        /// </summary>
        /// <param name="writer">Does Nothing. Don't Use.</param>
        [Obsolete]
        public virtual void Save(BinaryWriter writer)
        {
        }
    }
}