using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Abaki
{
    public partial class Form1 : Form
    {
        private string LanguageFile = "";

        public Form1()
        {
            InitializeComponent();
        }

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
                    var AmountOfStrings =
                        Convert.ToInt32(
                            Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0'))) - 1;
                    var JumpTo =
                        Convert.ToInt32(
                            Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0')));
                    var CurrentPos = Reader.BaseStream.Position;
                    Reader.BaseStream.Position = JumpTo - 4;
                    Reader.BaseStream.Position = CurrentPos;

                    var Count = 0;
                    var StringOffsets = new List<int>();
                    do
                    {
                        StringOffsets.Add(Convert.ToInt32(
                            Utilities.HexToDec(Utilities.ByteArrayToString(Reader.ReadBytes(4)).TrimStart('0'))));
                        Count++;
                    } while (Count < AmountOfStrings);

                    for (Count = 0; Count < StringOffsets.Count; Count++)
                        listBox1.Items.Add(Count != StringOffsets.Count - 1
                            ? Encoding.BigEndianUnicode.GetString(
                                Reader.ReadBytes(StringOffsets[Count] - (int) Reader.BaseStream.Position))
                            : Encoding.BigEndianUnicode.GetString(
                                Reader.ReadBytes((int) Reader.BaseStream.Length - (int) Reader.BaseStream.Position)));
                }
            }
        }

        private void ExportToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (listBox1.Items.Count == 0)
                MessageBox.Show("There is nothing to export.", "Nothing To Export!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            using (var Writer = new BinaryWriter(File.Open(LanguageFile, FileMode.Create, FileAccess.Write)))
            {
                Writer.Write(BitConverter.GetBytes(Utilities.SwapBytes((uint) listBox1.Items.Count + 1)));

                long Sum = listBox1.Items.Count * 4 + 4;
                foreach (var Item in listBox1.Items)
                {
                    Writer.Write(Utilities.SwapBytes((uint) Sum));
                    Sum = Sum + LanguageParse(Encoding.BigEndianUnicode.GetBytes(Item.ToString()));
                }

                foreach (var Item in listBox1.Items) Writer.Write(Encoding.BigEndianUnicode.GetBytes(Item.ToString()));
            }
        }

        //Please Rewrite me....
        public static long LanguageParse(byte[] Bytes)
        {
            var HexString = new StringBuilder(Bytes.Length * 2);
            foreach (var Byte in Bytes) HexString.Append(Byte == 0x00 ? '.' : Convert.ToChar(Byte));

            return HexString.ToString().Length;
        }

        private void Button1_Click(object Sender, EventArgs Args)
        {
            if (textBox1.Text.Length == 0)
                listBox1.Items[listBox1.SelectedIndex] = "";

            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
        }

        private void ListBox1_SelectedIndexChanged(object Sender, EventArgs Args)
        {
            textBox1.Text = listBox1.GetItemText(listBox1.SelectedItem);
        }

        private void quitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Application.Exit();
        }
    }
}