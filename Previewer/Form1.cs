using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Previewer
{
    public partial class Form1 : Form
    {
        private char CurrentLanguage = 'E'; //Default To English.
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (!Directory.Exists("YGO_DATA"))
                MessageBox.Show("YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", "YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                OFD.Filter = "Card Indx File (*.bin) | *.bin";

                if (OFD.ShowDialog() != DialogResult.OK)
                    return;

                if (!OFD.SafeFileName.StartsWith("CARD_Indx"))
                {
                    MessageBox.Show($"Invalid File, Card Index Should Be Called \"CARD_Indx_{CurrentLanguage}.bin\"", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                using (var IndexReader = new BinaryReader(File.Open(OFD.FileName, FileMode.Open, FileAccess.Read)))
                {
                    IndexReader.BaseStream.Position += 0x8; //We're Only Reading 4 Bytes. (File Header Structure)
                    var NameDescIndx = new Dictionary<int, int>();

                    do
                    {
                        Debug.WriteLine($"Offset: {IndexReader.BaseStream.Position}: {Utilities.ByteArrayToString(IndexReader.ReadBytes(0x4))} - {Utilities.ByteArrayToString(IndexReader.ReadBytes(0x4))}");
                    } while (IndexReader.BaseStream.Position < IndexReader.BaseStream.Length);
                }
            }
        }

        private void deckToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            
        }

        private void LanguageToolStripMenuItem_Click(object sender, System.EventArgs Args)
        {
            foreach (ToolStripMenuItem Language in languageToolStripMenuItem.DropDownItems)
                Language.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true;
            CurrentLanguage = ((ToolStripMenuItem) sender).Text[0];
        }
    }
}