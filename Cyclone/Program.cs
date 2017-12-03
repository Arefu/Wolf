using System;
using System.IO;
using System.Windows.Forms;

namespace Cyclone
{
    public class Cyclone
    {
        [STAThread]
        private static void Main()
        {
            //Load ZIB.
            var ZibFolder = "";
            using (var Fbd = new FolderBrowserDialog())
            {
                Fbd.ShowNewFolderButton = false;
                Fbd.Description = "Select the ZIB folder.";
                Fbd.SelectedPath = Application.StartupPath;

                if (Fbd.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1);
                else
                    ZibFolder = Fbd.SelectedPath;
            }

            switch (new DirectoryInfo(ZibFolder).Name.Replace(" Unpacked", string.Empty))
            {
                case "busts.zib":
                    Packer.Pack(ZibFolder, 0x2390);
                    break;
                case "cardcropHD400.jpg.zib":
                    Packer.Pack(ZibFolder);
                    break;
                case "cardcropHD401.jpg.zib":
                    Packer.Pack(ZibFolder);
                    break;
                case "decks.zib":
                    Packer.Pack(ZibFolder, 0x8650);
                    break;
                case "packs.zib":
                    Packer.Pack(ZibFolder, 0x750);
                    break;
                default:
                    throw new Exception("This is either an unsupported ZIB, or a corrupted extraction.");
            }
        }
    }
}