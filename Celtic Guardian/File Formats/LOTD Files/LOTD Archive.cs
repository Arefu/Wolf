using System.IO;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_Archive
    {
        public BinaryReader Reader { get; private set; }
        public LOTD_Directory Root { get; private set; }
    }
}