using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Abaki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string LanguageFile = "";

        private void LoadToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            listBox1.Items.Clear();

            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select File To Decode";
                Ofd.Filter = "Language file (*.bnd) | *.bnd";
                if (Ofd.ShowDialog() != DialogResult.OK) return;

                LanguageFile = Ofd.SafeFileName;
                using (var Reader = new BinaryReader(File.Open(Ofd.FileName, FileMode.Open, FileAccess.Read)))
                {
                    var AmountOfStrings = Convert.ToInt32(Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0'))) - 1;
                    var JumpTo = Convert.ToInt32(Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0')));
                    var CurrentPos = Reader.BaseStream.Position;
                    Reader.BaseStream.Position = JumpTo - 4;
                    var LastOffset = Convert.ToInt32(Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0')));
                    Reader.BaseStream.Position = CurrentPos;

                    var Count = 0;
                    var StringOffsets = new List<int>();
                    do
                    {
                        StringOffsets.Add(Convert.ToInt32(Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0'))));
                        Count++;
                    } while (Count < AmountOfStrings);

                    for (Count = 0; Count < StringOffsets.Count; Count++)
                    {
                        if (Count != StringOffsets.Count - 1)
                        {
                            if ((int)Reader.BaseStream.Position > StringOffsets[Count + 1])
                                listBox1.Items.Add(Utilities.GetText(Reader.ReadBytes((int)Reader.BaseStream.Position - StringOffsets[Count + 1])));
                            else
                                listBox1.Items.Add(Utilities.GetText(Reader.ReadBytes(StringOffsets[Count] - (int)Reader.BaseStream.Position)));
                        }
                        else
                            listBox1.Items.Add(Utilities.GetText(Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position)));
                    }
                }
            }
        }

        private void ExportToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            using (var Writer = new BinaryWriter(File.Open(LanguageFile, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                Writer.Write(new byte[] { 0x00, 0x00});
                Writer.Write(Encoding.Default.GetBytes(listBox1.Items.Count.ToString()));

                foreach (var Item in listBox1.Items)
                {
                    foreach (var Char in Item.ToString())
                    {
                        Writer.Write(new byte[] { Convert.ToByte(Char), 0x00 });
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.GetItemText(listBox1.SelectedItem);
        }
    }
}