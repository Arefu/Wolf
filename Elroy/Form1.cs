using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elroy
{
    public partial class Form1 : Form
    {
        private static string SaveFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void opemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var OFD = new OpenFileDialog())
            {
                OFD.Title = "Select Your Save File...";
                OFD.Multiselect = false;
                OFD.Filter = "Save file (*.dat) | *.dat";

                if (OFD.ShowDialog() != DialogResult.OK)
                    return;


                //Start Parsing Save File Populating Items.
                //Check If Save File.
                using (var Reader = new BinaryReader(File.Open(OFD.FileName, FileMode.Open, FileAccess.Read)))
                {
                    var SaveHeader = Reader.ReadBytes(10);
                    var KnownHeader = new byte[] {0xF9, 0x29, 0xCE, 0x54, 0x02, 0x4D, 0x71, 0x04, 0x4D, 0x71};

                    if (!KnownHeader.SequenceEqual(SaveHeader))
                    {
                        MessageBox.Show("This Is Either A Corrupted Save, Or Not A Save File. Refer To Wiki For Information", "Invalid Save!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Reader.Close();
                    tabControl1.Enabled = true;
                    SaveFile = OFD.FileName;
                }

                SaveStatManager.UpdateSaveStatFromSave(SaveFile);
                StoryManager.UpdateCampaignFromSave(ref tabPage4, SaveFile);
            }
        }

        private void DeckEditButton_Click(object Sender, EventArgs Args)
        {
            var SenderButton = (Button) Sender;
            var Manager = new DeckManager(SenderButton.Name, SaveFile);
            Manager.ShowDialog();
        }

        private void SaveToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (!File.Exists(SaveFile)) return;
            var Save = new GameSaveData(SaveFile);

            SaveStatManager.WriteSaveStatToSave(SaveFile);
            //StoryManager.WriteCampaignToSave(ref tabPage4,SaveFile);

            Save.FixGameSaveSignatureOnDisk();
        }


       

        private void button34_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Will Unlock ALL Cards. You Need To Own DLC Cards Before You Can Use Them!", "Unlock ALL Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}