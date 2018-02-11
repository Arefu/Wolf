using System;
using System.IO;
using System.Linq;
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

                //UpdateSaveStat Info
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
            if (!File.Exists(SaveFile)) return;
            var Save = new GameSaveData(SaveFile);
            Save.FixGameSaveSignatureOnDisk();
        }

        private void Achievement_ValueChanged(object Sender, EventArgs Args)
        {
            var SaveStatOffset = 0x0L;
            switch (((NumericUpDown)Sender).Name)
            {
                case "SAVESTAT_GAMES_CAMPAIGN":
                    // 24 - 2B
                    SaveStatOffset = 0x24;
                    break;
                case "SAVESTAT_GAMES_CAMPAIGN_NORMAL":
                    // 2C - 33
                    SaveStatOffset = 0x2C;
                    break;
                case "SAVESTAT_GAMES_CAMPAIGN_REVERSE":
                    // 34 - 3B
                    SaveStatOffset = 0x34;
                    break;
                case "SAVESTAT_GAMES_CHALLENGE":
                    // 3C - 43
                    SaveStatOffset = 0x3C;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER":
                    // 44 - 4B
                    SaveStatOffset = 0x44;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_1V1":
                    // 4C - 53
                    SaveStatOffset = 0x4C;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_TAG":
                    // 54 - 5B
                    SaveStatOffset = 0x54;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_RANKED":
                    // 5C - 63
                    SaveStatOffset = 0x5C;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_FRIENDLY":
                    // 64 - 6B
                    SaveStatOffset = 0x64;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_ANY":
                    // 6C - 73
                    SaveStatOffset = 0x6C;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_1":
                    // 74 - 7B
                    SaveStatOffset = 0x74;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_2":
                    // 7C - 83
                    SaveStatOffset = 0x7C;
                    break;
                case "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_3":
                    // 84 - 8B
                    SaveStatOffset = 0x84;
                    break;
                case "SAVESTAT_WINS_CAMPAIGN":
                    // 8C - 93
                    SaveStatOffset = 0x8C;
                    break;
                case "SAVESTAT_WINS_CAMPAIGN_NORMAL":
                    // 94 - 9B
                    SaveStatOffset = 0x94;
                    break;
                case "SAVESTAT_WINS_CAMPAIGN_REVERSE":
                    // 9C - A3
                    SaveStatOffset = 0x9C;
                    break;
                case "SAVESTAT_WINS_CHALLENGE":
                    // A4 - AB
                    SaveStatOffset = 0xA4;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER":
                    // AC - B3
                    SaveStatOffset = 0xAC;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_1V1":
                    // B4 - BB
                    SaveStatOffset = 0xB4;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_TAG":
                    // BC - C3
                    SaveStatOffset = 0xBC;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_RANKED":
                    // C4 - CB
                    SaveStatOffset = 0xC4;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_FRIENDLY":
                    // CC - D3
                    SaveStatOffset = 0xCC;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_ANY":
                    // D4 - D8
                    SaveStatOffset = 0xD4;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_1":
                    // DC - E3
                    SaveStatOffset = 0xDC;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_2":
                    // E4 - EB
                    SaveStatOffset = 0xE4;
                    break;
                case "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_3":
                    // EC - F3
                    SaveStatOffset = 0xEC;
                    break;
                case "SAVESTAT_WINS_MATCH":
                    // F4 - FB
                    SaveStatOffset = 0xF4;
                    break;
                case "SAVESTAT_WINS_NONMATCH":
                    // FC - 103
                    SaveStatOffset = 0xFC;
                    break;
                case "SAVESTAT_SUMMONS_NORMAL":
                    // 104 - 10B
                    SaveStatOffset = 0x104;
                    break;
                case "SAVESTAT_SUMMONS_TRIBUTE":
                    // 10C - 113
                    SaveStatOffset = 0x10C;
                    break;
                case "SAVESTAT_SUMMONS_RITUAL":
                    // 114 - 11B
                    SaveStatOffset = 0x114;
                    break;
                case "SAVESTAT_SUMMONS_FUSION":
                    // 11C - 123
                    SaveStatOffset = 0x11C;
                    break;
                case "SAVESTAT_SUMMONS_XYZ":
                    // 124 - 12B
                    SaveStatOffset = 0x124;
                    break;
                case "SAVESTAT_SUMMONS_SYNCHRO":
                    // 12C - 133
                    SaveStatOffset = 0x12C;
                    break;
                case "SAVESTAT_SUMMONS_PENDULUM":
                    // 134 - 13B
                    SaveStatOffset = 0x134;
                    break;
                case "SAVESTAT_DAMAGE_ANY":
                    // 13C - 143
                    SaveStatOffset = 0x13C;
                    break;
                case "SAVESTAT_DAMAGE_BATTLE":
                    // 144 - 14B
                    SaveStatOffset = 0x144;
                    break;
                case "SAVESTAT_DAMAGE_DIRECT":
                    // 14C - 153
                    SaveStatOffset = 0x14C;
                    break;
                case "SAVESTAT_DAMAGE_EFFECT":
                    // 154 - 15B
                    SaveStatOffset = 0x154;
                    break;
                case "SAVESTAT_DAMAGE_REFLECT":
                    // 15C - 163
                    SaveStatOffset = 0x15C;
                    break;
                case "SAVESTAT_CHAINS":
                    // 164 - 16B
                    SaveStatOffset = 0x164;
                    break;
                case "SAVESTAT_DECKS_CREATED":
                    // 16C - 173
                    SaveStatOffset = 0x16C;
                    break;
                default:
                    throw new Exception("SAVESTAT Not Found!");
            }
            using (var SaveStatWriter = new BinaryWriter(File.Open(SaveFile, FileMode.Open, FileAccess.Write)))
            {
                SaveStatWriter.BaseStream.Position = SaveStatOffset;
                //Write Value Buffered To 8 Bytes With 0x00 Fill.
            }
        }
    }
}