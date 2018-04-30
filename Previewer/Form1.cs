using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int GlobalIndex;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object Sender, EventArgs Args)
        {
            if (Directory.Exists("YGO_DATA")) return;

            MessageBox.Show(@"YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", @"YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void ExitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Application.Exit();
        }

        private void CardIndexToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
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

        private void LoadCards()
        {
            var Index = GlobalIndex; //Dunno Why?
            using (var CardTitleReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Name_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
            {
                using (var CardDescReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Desc_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
                {
                    CardTitleReader.BaseStream.Position = NameIndx[Index];
                    CardDescReader.BaseStream.Position = DescIndx[Index];
                    if (CardTitleReader.BaseStream.Position < CardTitleReader.BaseStream.Length)
                    {
                        if (CardDescReader.BaseStream.Position < CardDescReader.BaseStream.Length)
                        {
                            var NameSum = NameIndx[Index + 1] - NameIndx[Index];
                            var DescSum = DescIndx[Index + 1] - DescIndx[Index];
                            cardTitle.Text = Utilities.GetText(CardTitleReader.ReadBytes(NameSum));
                            cardDesc.Text = Utilities.GetText(CardDescReader.ReadBytes(DescSum));
                        }
                    }   
                }
            }

            label5.Text = Index.ToString();
        }

        private void DeckToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
        }

        private void LanguageToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            foreach (ToolStripMenuItem Language in languageToolStripMenuItem.DropDownItems)
                Language.Checked = false;

            ((ToolStripMenuItem) Sender).Checked = true;
            CurrentLanguage = ((ToolStripMenuItem) Sender).Text[0];
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
            if (GlobalIndex >= 1)
            {
                GlobalIndex--;
                numericUpDown1.Value--;
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Fist Card", @"No More Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button1_Click_1(object Sender, EventArgs Args)
        {
            GlobalIndex = (int) numericUpDown1.Value;
            LoadCards();
        }

        private void CheckBox1_CheckedChanged(object Sender, EventArgs Args)
        {
            checkBox1.Checked = false;
        }
    }
}