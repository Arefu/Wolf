using System;
using System.IO;

namespace Celtic_Guardian
{
    [Obsolete("This Class Shouldn't Be Used, It Might Not Be Here Later In This Libraries Life.")]
    public class RawFile : File_Data
    {
        public byte[] Buffer { get; set; }

        public override void Load(BinaryReader reader, long length)
        {
            if (length <= int.MaxValue)
                Buffer = reader.ReadBytes((int) length);
            else
                throw new OverflowException("Length Is Larger Than Int32.MaxValue! CHECK CODE PLEASE!");
        }

        public override void Save(BinaryWriter writer)
        {
            if (Buffer == null) throw new IOException("Buffer Has No Content! CHECK CODE PLEASE!");

            writer.Write(Buffer);
        }
    }
}