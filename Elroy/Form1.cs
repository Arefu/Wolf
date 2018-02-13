using Celtic_Guardian;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elroy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static string SaveFile;

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
                    var KnownHeader = new byte[] { 0xF9, 0x29, 0xCE, 0x54, 0x02, 0x4D, 0x71, 0x04, 0x4D, 0x71 };

                    if (!KnownHeader.SequenceEqual(SaveHeader))
                    {
                        MessageBox.Show("This Is Either A Corrupted Save, Or Not A Save File. Refer To Wiki For Information", "Invalid Save!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Reader.Close();
                    tabControl1.Enabled = true;
                    SaveFile = OFD.FileName;
                }

                UpdateSaveStatFromSave();
            }
        }

        private void DeckEditButton_Click(object Sender, EventArgs Args)
        {
            var SenderButton = (Button)Sender;
            var Manager = new DeckManager(SenderButton.Name, SaveFile);
            Manager.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            WriteSaveStatToSave();
            if (!File.Exists(SaveFile)) return;
            var Save = new GameSaveData(SaveFile);
            Save.FixGameSaveSignatureOnDisk();
        }


        private void UpdateSaveStatFromSave()
        {
            using (var SaveStatReader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read)))
            {
                SaveStatReader.BaseStream.Position = 0x24;

                // "SAVESTAT_GAMES_CAMPAIGN"
                numericUpDown1.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_CAMPAIGN_NORMAL"
                numericUpDown2.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_CAMPAIGN_REVERSE"
                numericUpDown3.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_CHALLENGE"
                numericUpDown4.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER"
                numericUpDown5.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // ""SAVESTAT_GAMES_MULTIPLAYER_1V1"
                numericUpDown6.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_TAG"
                numericUpDown7.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_RANKED"
                numericUpDown8.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_FRIENDLY"
                numericUpDown9.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_ANY"
                numericUpDown10.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_1"
                numericUpDown11.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_2"
                numericUpDown12.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_3"
                numericUpDown13.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_CAMPAIGN"
                numericUpDown14.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_CAMPAIGN_NORMAL"
                numericUpDown15.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_CAMPAIGN_REVERSE"
                numericUpDown16.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_CHALLENGE"
                numericUpDown17.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER"
                numericUpDown18.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_1V1"
                numericUpDown19.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_TAG"
                numericUpDown20.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_RANKED"
                numericUpDown21.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_FRIENDLY"
                numericUpDown22.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_ANY"
                numericUpDown23.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_1"
                numericUpDown24.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_2"
                numericUpDown25.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_3"
                numericUpDown26.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_MATCH"
                numericUpDown27.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_WINS_NONMATCH"
                numericUpDown28.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_NORMAL"
                numericUpDown29.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_TRIBUTE"
                numericUpDown30.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_RITUAL"
                numericUpDown31.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_FUSION"
                numericUpDown32.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_XYZ"
                numericUpDown33.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_SYNCHRO"
                numericUpDown34.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_SUMMONS_PENDULUM"
                numericUpDown35.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_DAMAGE_ANY"
                numericUpDown36.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_DAMAGE_BATTLE"
                numericUpDown37.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // SAVESTAT_DAMAGE_DIRECT"
                numericUpDown38.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_DAMAGE_EFFECT"
                numericUpDown39.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_DAMAGE_REFLECT"
                numericUpDown40.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_CHAINS"
                numericUpDown41.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);

                // "SAVESTAT_DECKS_CREATED"
                numericUpDown42.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
            }
        }

        private void WriteSaveStatToSave()
        {
            using (var SaveStatWriter = new BinaryWriter(File.Open(SaveFile, FileMode.Open, FileAccess.Write)))
            {
                SaveStatWriter.BaseStream.Position = 0x24;

                // "SAVESTAT_GAMES_CAMPAIGN"

                // "SAVESTAT_GAMES_CAMPAIGN_NORMAL"

                // "SAVESTAT_GAMES_CAMPAIGN_REVERSE"

                // "SAVESTAT_GAMES_CHALLENGE"

                // "SAVESTAT_GAMES_MULTIPLAYER"

                // ""SAVESTAT_GAMES_MULTIPLAYER_1V1"

                // "SAVESTAT_GAMES_MULTIPLAYER_TAG"

                // "SAVESTAT_GAMES_MULTIPLAYER_RANKED"

                // "SAVESTAT_GAMES_MULTIPLAYER_FRIENDLY"

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_ANY"

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_1"

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_2"

                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_3"

                // "SAVESTAT_WINS_CAMPAIGN"

                // "SAVESTAT_WINS_CAMPAIGN_NORMAL"

                // "SAVESTAT_WINS_CAMPAIGN_REVERSE"

                // "SAVESTAT_WINS_CHALLENGE"

                // "SAVESTAT_WINS_MULTIPLAYER"

                // "SAVESTAT_WINS_MULTIPLAYER_1V1"

                // "SAVESTAT_WINS_MULTIPLAYER_TAG"

                // "SAVESTAT_WINS_MULTIPLAYER_RANKED"

                // "SAVESTAT_WINS_MULTIPLAYER_FRIENDLY"

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_ANY"

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_1"

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_2"

                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_3"

                // "SAVESTAT_WINS_MATCH"

                // "SAVESTAT_WINS_NONMATCH"

                // "SAVESTAT_SUMMONS_NORMAL"

                // "SAVESTAT_SUMMONS_TRIBUTE"

                // "SAVESTAT_SUMMONS_RITUAL"

                // "SAVESTAT_SUMMONS_FUSION"

                // "SAVESTAT_SUMMONS_XYZ"

                // "SAVESTAT_SUMMONS_SYNCHRO"

                // "SAVESTAT_SUMMONS_PENDULUM"

                // "SAVESTAT_DAMAGE_ANY"

                // "SAVESTAT_DAMAGE_BATTLE"

                // SAVESTAT_DAMAGE_DIRECT"

                // "SAVESTAT_DAMAGE_EFFECT"

                // "SAVESTAT_DAMAGE_REFLECT"

                // "SAVESTAT_CHAINS"

                // "SAVESTAT_DECKS_CREATED"

            }
        }

        private void UpdateCampaignFromSave()
        {
            using (var CampaignReader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read)))
            {
                CampaignReader.BaseStream.Position = 0x24;

            }
        }

        private void WriteCampaignToSave()
        {

        }

        private void button34_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Will Unlock ALL Cards. You Need To Own DLC Cards Before You Can Use Them!", "Unlock ALL Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}