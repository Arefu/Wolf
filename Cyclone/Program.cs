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
                    break;
                case "cardcropHD400.jpg.zib":
                    CardCrop.Extract(4, 4, 39, 40, 9, ZibFolder);
                    break;
                case "cardcropHD401.jpg.zib":
                    CardCrop.Extract(4, 4, 39, 40, 9, ZibFolder);
                    break;
                case "decks.zib":
                    break;
                case "packs.zib":
                    break;
            }
        }
    }
}