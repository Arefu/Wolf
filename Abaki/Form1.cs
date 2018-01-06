using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
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
                            //Un-Comment Once RePacking Logic Is Done.
                            //{
                            //    if ((int)Reader.BaseStream.Position > StringOffsets[Count + 1])
                            //        listBox1.Items.Add(Utilities.GetText(Reader.ReadBytes((int)Reader.BaseStream.Position - StringOffsets[Count + 1])));
                            //    else
                            listBox1.Items.Add(System.Text.Encoding.BigEndianUnicode.GetString(Reader.ReadBytes(StringOffsets[Count] - (int)Reader.BaseStream.Position)));
                        //}
                        else
                            listBox1.Items.Add(System.Text.Encoding.BigEndianUnicode.GetString(Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position)));
                    }
                }
            }
        }

        private void ExportToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (listBox1.Items.Count == 0)
                return;

            using (var Writer = new BinaryWriter(File.Open(LanguageFile, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                Writer.Write(BitConverter.GetBytes(Utilities.SwapBytes((uint)listBox1.Items.Count + 1))); //Might Not Need To Be + 1?

                long Sum = listBox1.Items.Count * 4;
                //LanguageParse(Encoding.BigEndianUnicode.GetBytes(Item.ToString()));
                foreach (var Item in listBox1.Items)
                {
                    var Offset = 0L;
                    Sum = Sum + LanguageParse(Encoding.BigEndianUnicode.GetBytes(Item.ToString()));
                    Writer.Write(Utilities.SwapBytes((uint)Sum));
                    Offset = Writer.BaseStream.Position;
                    Writer.BaseStream.Position = Sum;
                    Writer.Write(Encoding.BigEndianUnicode.GetBytes(Item.ToString()));
                    Writer.BaseStream.Position = Offset;
                }
            }
        }

        //Please Rewrite me....
        public static long LanguageParse(byte[] Bytes)
        {
            var HexString = new StringBuilder(Bytes.Length * 2);
            foreach (var Byte in Bytes)
            {
                if (Byte == 0x00)
                {
                    HexString.Append('.');
                }
                else
                {
                    HexString.Append(Convert.ToChar(Byte));
                }
            }

            return HexString.ToString().Length;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                listBox1.Items[listBox1.SelectedIndex] = "";

            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.GetItemText(listBox1.SelectedItem);
        }
    }
}