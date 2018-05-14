using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Previewer
{
    public partial class Form1 : Form
    {
        private readonly List<int> DescIndx = new List<int>();
        private readonly List<int> NameIndx = new List<int>();
        private char CurrentLanguage = 'E'; //Default To English.
        private string CurrentUnpackedImagesDir = String.Empty;
        private int GlobalIndex = 1;
        internal static BinaryReader CardPropReader= new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Prop.bin", FileMode.Open, FileAccess.Read));

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object Sender, EventArgs Args)
        {
            if (Directory.Exists("YGO_DATA"))
            {
               // CardPropReader ;
                return;
            }

            MessageBox.Show(@"YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", @"YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void ExitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Application.Exit();
        }

        private void CardIndexToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (CurrentUnpackedImagesDir == string.Empty)
            {
                using (var FolderSelect = new FolderBrowserDialog())
                {
                    FolderSelect.Description = @"Please Select Your 'cardcropHD4XX.jpg.zib Unpacked' Folder!";
                    FolderSelect.SelectedPath = Application.StartupPath;
                    var Reply = FolderSelect.ShowDialog();
                    if (Reply != DialogResult.OK) return;

                    CurrentUnpackedImagesDir = FolderSelect.SelectedPath;
                }
            }

            CardPropReader.BaseStream.Position += 0x8;

            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = @"Select Card Index File";
                FileDialog.Filter = $@"Card Indx File (CARD_Indx_{CurrentLanguage}.bin) | *CARD_Indx_{CurrentLanguage}.bin";

                if (FileDialog.ShowDialog() != DialogResult.OK)
                    return;

                using (var IndexReader = new BinaryReader(File.Open(FileDialog.FileName, FileMode.Open, FileAccess.Read)))
                {
                    IndexReader.BaseStream.Position += 0x8; //We're Only Reading 4 Bytes. (File Header Structure)

                    do
                    {
                        NameIndx.Add(Utilities.ConvertToLittleEndian(IndexReader.ReadBytes(0x4), 0));
                        DescIndx.Add(Utilities.ConvertToLittleEndian(IndexReader.ReadBytes(0x4), 0));
                    } while (IndexReader.BaseStream.Position < IndexReader.BaseStream.Length);
                }

                label4.Text = $@"{NameIndx.Count - 1}";
                
                LoadCards();
            }

            numericUpDown1.Maximum = NameIndx.Count + 1;
            numericUpDown1.Minimum = 1;
        }

        struct CardTemplate
        {
            internal uint Atk;
            internal uint Def;
            internal uint Level;
            internal short Id;
        };

        private void LoadCards()
        {
            using (var CardTitleReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Name_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
            {
                using (var CardDescReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Desc_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
                {
                    CardTitleReader.BaseStream.Position = NameIndx[GlobalIndex-1];
                    CardDescReader.BaseStream.Position = DescIndx[GlobalIndex-1];

                    CardPropReader.BaseStream.Position = GlobalIndex * 8;

                    if (CardTitleReader.BaseStream.Position < CardTitleReader.BaseStream.Length)
                    {
                        if (CardDescReader.BaseStream.Position < CardDescReader.BaseStream.Length)
                        {
                            var NameSum = NameIndx[GlobalIndex + 1] - NameIndx[GlobalIndex];
                            var DescSum = DescIndx[GlobalIndex + 1] - DescIndx[GlobalIndex];


                            var Card = ByteJumbo(CardPropReader.ReadUInt32(), CardPropReader.ReadUInt32());

                            cardTitle.Text = Utilities.GetText(CardTitleReader.ReadBytes(NameSum));
                            cardDesc.Text = Utilities.GetText(CardDescReader.ReadBytes(DescSum));
                            pictureBox1.Image = Image.FromFile($"{CurrentUnpackedImagesDir}/{Card.Id}.jpg");
                            AtkTextBox.Text = Card.Atk.ToString();
                            DefTextBox.Text = Card.Def.ToString();
                            textBox1.Text = Card.Id.ToString();
                        }
                    }
                }
            }

            label5.Text = GlobalIndex.ToString();
        }

        private static CardTemplate ByteJumbo(uint FirstChunk, uint SecondChunk)
        {
            var First = (FirstChunk << 18) | ((FirstChunk & 0x7FC000 | FirstChunk >> 18) >> 5);
            var Second = ((SecondChunk & 1u) | (SecondChunk << 21)) & 0x80000001 | ((SecondChunk & 0x7800) | ((SecondChunk & 0x780 | ((SecondChunk & 0x7E) << 10)) << 8)) << 6 | ((SecondChunk & 0x38000 | ((SecondChunk & 0x7C0000 | ((SecondChunk & 0x7800000 | (SecondChunk >> 8) & 0x780000) >> 9)) >> 8)) >> 1);

            var CardId = (short)((First >> 18) & 0x3FFF);
            var Atk = (First >> 9) & 0x1FF;
            var Def = First & 0x1FF;
            var Level = (Second >> 17) & 0xF;

            CardTemplate Card;
            Card.Id = CardId;
            Card.Atk = Atk*10;
            Card.Def = Def*10;
            Card.Level = Level;
            return Card;
        }

        private void DeckToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
        }

        private void LanguageToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            foreach (ToolStripMenuItem Language in languageToolStripMenuItem.DropDownItems)
                Language.Checked = false;

            ((ToolStripMenuItem)Sender).Checked = true;
            CurrentLanguage = ((ToolStripMenuItem)Sender).Text[0];
        }

        private void Button1_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex <= NameIndx.Count)
            {
                GlobalIndex++;
                numericUpDown1.Value++;
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Last Card", @"No More Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PrevCard_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex >= 0)
            {
                GlobalIndex--;
                numericUpDown1.Value--;
                CardPropReader.BaseStream.Position -= 16;
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Fist Card", @"No More Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button1_Click_1(object Sender, EventArgs Args)
        {
            GlobalIndex = (int)numericUpDown1.Value;
            LoadCards();
        }

        private void CheckBox1_CheckedChanged(object Sender, EventArgs Args)
        {
            checkBox1.Checked = false;
        }
    }
}