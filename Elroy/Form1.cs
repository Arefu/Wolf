using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elroy
{
    public partial class Form1 : Form
    {
        public static BinaryReader SaveReader;

        public Form1()
        {
            InitializeComponent();
            FileLoadedStatusLabel.Text = "Not Loaded";
            FileLoadedStatusLabel.ForeColor = Color.Red;
        }

        private void panel1_DragDrop(object Sender, DragEventArgs Args)
        {
            var FileList = (string[]) Args.Data.GetData(DataFormats.FileDrop, true);
            try
            {
                var SaveFile = FileList.First(File => File.EndsWith("savegame.dat"));
            }
            catch (Exception)
            {
                using (var OFD = new OpenFileDialog())
                {
                    OFD.Title = "Couldn't Locate Save File, Please Navigate To It...";
                }
            }
            //if (SaveFile == null)
            //{
            //    MessageBox.Show("Save File Not Found!");
            //    return;
            //}
            //using (SaveReader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read)))
            //{
            //    var SaveHeader = SaveReader.ReadBytes(16);
            //    var KnownSaveHeader = new byte[] {0xF9, 0x29, 0xCE, 0x54, 0x02, 0x4D, 0x71, 0x04, 0x4D, 0x71, 0x00, 0x00, 0xE1, 0xB7, 0x2A, 0x43 };
            //    if (SaveHeader.SequenceEqual(KnownSaveHeader))
            //        MessageBox.Show("");

            //}
            //    FileLoadedStatusLabel.Text = "Loaded Save File";
            //FileLoadedStatusLabel.ForeColor = Color.Green;
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}