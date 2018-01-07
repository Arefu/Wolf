using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

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
                        MessageBox.Show("This Is Either A Corrupted Save, Or Not A Save File.", "Invalid Save!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Reader.Close();

                    GetDeckNames(OFD.FileName);
                }
            }
        }

        private void GetDeckNames(string SaveFile)
        {
            var DeckNames = new Dictionary<int, string>();
            using (var Reader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read)))
            {
                var Count = 0;
                Reader.BaseStream.Position = 0x2DB0;
                do
                {
                    var Name = Utilities.GetText(Reader.ReadBytes(0x40));
                    if (string.IsNullOrEmpty(Name))
                    {
                        Count++;
                        continue;
                    }

                    DeckNames.Add(Count, Name);
                    Reader.BaseStream.Position += 0xF0;
                    Count++;
                } while (Count <= 32);
            }

            foreach (var Deck in DeckNames)
            {
                foreach (var Box in tabControl1.SelectedTab.Controls)
                {
                    MessageBox.Show(Box.ToString());
                }
            }
        }

        private void DeckEditButton_Click(object Sender, EventArgs Args)
        {
            var SenderButton = (Button)Sender;
            var Manager = new DeckManager(SenderButton.Name);
            Manager.ShowDialog();
        }
    }
}