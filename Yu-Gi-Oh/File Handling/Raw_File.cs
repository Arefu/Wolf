using System;
using System.IO;

namespace Yu_Gi_Oh.File_Handling
{
    [Obsolete("This Class Shouldn't Be Used, It SHould Only Be Inherited.")]
    public class Raw_File : File_Data
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