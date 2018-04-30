using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Previewer
{
    public partial class Form1 : Form
    {
        //Name -> Desc
        private List<int> NameIndx = new List<int>();
        private List<int> DescIndx = new List<int>();

        private char CurrentLanguage = 'E'; //Default To English.
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (!Directory.Exists("YGO_DATA"))
            {
                MessageBox.Show("YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", "YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void CardIndexToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var OFD = new OpenFileDialog())
            {
                OFD.Title = "Select Card Index File";
                OFD.Filter = $"Card Indx File (CARD_Indx_{CurrentLanguage}.bin) | *CARD_Indx_{CurrentLanguage}.bin";

                if (OFD.ShowDialog() != DialogResult.OK)
                    return;

                using (var IndexReader = new BinaryReader(File.Open(OFD.FileName, FileMode.Open, FileAccess.Read)))
                {
                    IndexReader.BaseStream.Position += 0x8; //We're Only Reading 4 Bytes. (File Header Structure)

                    do
                    {
                        NameIndx.Add(Utilities.ConvertToLittleEndian(IndexReader.ReadBytes(0x4),0));
                        DescIndx.Add(Utilities.ConvertToLittleEndian(IndexReader.ReadBytes(0x4),0));
                    } while (IndexReader.BaseStream.Position < IndexReader.BaseStream.Length);
                }
                label4.Text = $"{NameIndx.Count-1}";
                LoadCards();
                label5.Text = "1"; //Hackish.
            }

            numericUpDown1.Maximum = NameIndx.Count + 1;
            numericUpDown1.Minimum = 1;
        }

        private int GlobalIndex = 0;
        private void LoadCards()
        {
            var Index = GlobalIndex; //Dunno Why?
            using (var CardTitleReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Name_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
            {
                using (var CardDescReader = new BinaryReader(File.Open($"YGO_DATA/bin/CARD_Desc_{CurrentLanguage}.bin", FileMode.Open, FileAccess.Read)))
                {
                    CardTitleReader.BaseStream.Position = NameIndx[Index];
                    CardDescReader.BaseStream.Position = DescIndx[Index];
                    if(CardTitleReader.BaseStream.Position < CardTitleReader.BaseStream.Length)
                    {
                        if(CardDescReader.BaseStream.Position < CardDescReader.BaseStream.Length)
                        {
                            var NameSum = NameIndx[Index + 1] - NameIndx[Index];
                            var DescSum = DescIndx[Index + 1] - DescIndx[Index];
                            cardTitle.Text = Utilities.GetText(CardTitleReader.ReadBytes(NameSum));
                            cardDesc.Text = Utilities.GetText(CardDescReader.ReadBytes(DescSum));
                        }
                    }
                    else
                    {

                    }
                }
            }
            label5.Text = Index.ToString();
        }

        private void deckToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void LanguageToolStripMenuItem_Click(object sender, System.EventArgs Args)
        {
            foreach (ToolStripMenuItem Language in languageToolStripMenuItem.DropDownItems)
                Language.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true;
            CurrentLanguage = ((ToolStripMenuItem)sender).Text[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GlobalIndex <= NameIndx.Count)
            {
            GlobalIndex++;
            numericUpDown1.Value++;
            LoadCards();
            }
            else
            {
                MessageBox.Show("You're At The Last Card", "No More Cards!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void prevCard_Click(object sender, EventArgs e)
        {
            if(GlobalIndex >= 1)
            {
                GlobalIndex--;
                numericUpDown1.Value--;
                LoadCards();
            }
            else
            {
                MessageBox.Show("You're At The Fist Card", "No More Cards!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            GlobalIndex = (int)numericUpDown1.Value;
            LoadCards();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }
    }
}