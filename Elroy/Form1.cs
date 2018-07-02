using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.LOTD_Files;
using Yu_Gi_Oh.File_Handling.Main_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.Save_File;

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
                        MessageBox.Show(
                            "This Is Either A Corrupted Save, Or Not A Save File. Refer To Wiki For Information",
                            "Invalid Save!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Reader.Close();
                    tabControl1.Enabled = true;
                    SaveFile = OFD.FileName;
                }
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
            Save.FixGameSaveSignatureOnDisk();
        }

        private void DLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var MainFile = new LOTD_Archive(true);
            MainFile.Load();

            var Writer = new BinaryWriter(MainFile.Reader.BaseStream);
            var CharacterDataFiles = MainFile.LoadFiles<Char_Data>();
            var DuelDataFiles = MainFile.LoadFiles<Duel_Data>();

            foreach (var Character in CharacterDataFiles)
            {
                foreach (var CharacterItem in Character.Items.Values)
                    CharacterItem.DlcId = -1; //DLC Not Required.

                Writer.BaseStream.Position = Character.File.ArchiveOffset;
                Character.Save(Writer);
            }

            foreach (var Duel in DuelDataFiles)
            {
                foreach (var DuelItem in Duel.Items.Values)
                    DuelItem.DlcId = -1; //DLC Not Reuqired.

                Writer.BaseStream.Position = Duel.File.ArchiveOffset;
                Duel.Save(Writer);
            }

            Writer.Close();
            var save = new Game_Save();
            save.Load();
            save.SetAllCampaignDuels(CampaignDuelState.Complete);
            save.Save();
            MessageBox.Show("Done Unlocking All DLC!\nYou Will Need To Play Through The Story To Unlock The Duels.", "All Content Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void unlockOtherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var man = new Manager();
            man.Load();
            var save = new Game_Save();
            save.Load();
            save.UnlockPadlockedContent();
            save.UnlockAllAvatars();
            save.SetAllOwnedCardsCount(3, true);
            save.UnlockAllRecipes();
            save.SetAllCampaignDuels(CampaignDuelState.Complete);
            save.Save();
            MessageBox.Show("Done Unlocking All Content!\nYou Will Need To Play Through The Story To Unlock The Duels.", "All Content Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);}

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnlockCards(1);
        }

        private void ofEachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnlockCards(2);
        }

        private void ofEachToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UnlockCards(3);
        }

        private void UnlockCards(int Count)
        {
            var man = new Manager();
            man.Load();
            var save = new Game_Save();
            save.SetAllOwnedCardsCount((byte) Count);
            //save.UnlockAllCards();
            save.Save();
        }
    }
}