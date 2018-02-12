using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Elroy
{
    public partial class DeckManager : Form
    {
        private readonly string Save;
        private const long DeckStartOffset = 0x2C80; //0x2DB0 Should Be The First Deck;
        private readonly int DeckCode;
        private bool SomethingChanges;

        public DeckManager(string DeckName, string SavePath)
        {
            InitializeComponent();
            Save = SavePath;
            DeckCode = Convert.ToInt32(DeckName.Replace("button", string.Empty)) * 0x130;

            GetDeckInfo();

            textBox1.Text = DeckInfo.Name;
            label4.Text = $@"{DeckInfo.CardCount_Main} / {DeckInfo.CardCount_Extra} / {DeckInfo.CardCount_Side}";
        }

        private void ImportDeck_Click(object sender, EventArgs e)
        {
            var Result = MessageBox.Show("This WILL Overwrite Any Deck In This Slot, If You Want To Keep It Export The Deck First! Do You Wish To Continue?", "Warning Overwriting Deck!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.No) return;

            using (var Writer = new BinaryWriter(File.Open(Save, FileMode.Open, FileAccess.Write)))
            {
                Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                Writer.Write(new byte[0x130]);

                using (var OFD = new OpenFileDialog())
                {
                    OFD.Title = "Select Your YDC Deck File";
                    OFD.Filter = "Deck file (*.ydc) | *.ydc";
                    if (OFD.ShowDialog() != DialogResult.OK) return;
                    if (File.ReadAllBytes(OFD.FileName).Length > 0xC0) throw new IOException("YDC File To Big!");

                    Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                    Writer.Write(Encoding.BigEndianUnicode.GetBytes(new FileInfo(OFD.FileName).Name));
                    Writer.Write(new byte[0x30 - Encoding.BigEndianUnicode.GetBytes(new FileInfo(OFD.FileName).Name).Skip(1).ToArray().Length / 2]);
                    Writer.Write(File.ReadAllBytes(OFD.FileName));
                }
                Writer.Close();
            }
        }

        private void ExportDeck_Click(object sender, EventArgs e)
        {
            //Better Progress Needs To Be Made Here.
            //Can't Export Properly, can't reimport.. Etc.

            var DeckName = DeckInfo.Name;
            if (string.IsNullOrEmpty(DeckName))
            {
                MessageBox.Show("I Can't Export An Empty Deck.", "Deck Is Empty!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            File.Create($"{DeckName.ToLower().Replace(' ', '_')}.ydc").Close(); //Ready For Importing To Save, AI importing requires a bit of handy work from me. (I think)

            using (var Reader = new BinaryReader(File.Open(Save, FileMode.Open, FileAccess.Read)))
            {
                using (var Writer = new BinaryWriter(File.Open($"{DeckName.ToLower().Replace(' ', '_')}.ydc", FileMode.Open, FileAccess.Write)))
                {
                    Reader.BaseStream.Position = DeckStartOffset + DeckCode + 0x40;
                    Writer.Write(Reader.ReadBytes(0xC0));
                    Writer.Close();
                    Reader.Close();
                }
            }

            MessageBox.Show("Export Complete!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GetDeckInfo()
        {
            using (var Reader = new BinaryReader(File.Open(Save, FileMode.Open, FileAccess.Read)))
            {
                Reader.BaseStream.Position = DeckStartOffset + DeckCode;
                var DeckChunk = Reader.ReadBytes(0x130);
                var DeckName = Utilities.GetText(DeckChunk.Take(0x40).ToArray());
                if (string.IsNullOrEmpty(DeckName))
                {
                    DeckInfo.Name = "N/A";
                }
                else
                {
                    DeckInfo.Name = DeckName;
                    textBox1.Enabled = true;
                    Export_Deck.Enabled = true;
                }

                DeckChunk = DeckChunk.Skip(0x42).ToArray();
                DeckInfo.CardCount_Main = Utilities.HexToDec(DeckChunk.Take(1).ToArray());
                DeckChunk = DeckChunk.Skip(0x2).ToArray();
                DeckInfo.CardCount_Extra = Utilities.HexToDec(DeckChunk.Take(1).ToArray());
                DeckChunk = DeckChunk.Skip(0x2).ToArray();
                DeckInfo.CardCount_Side = Utilities.HexToDec(DeckChunk.Take(1).ToArray());

                Reader.Close();
            }
        }

        protected override void OnClosing(CancelEventArgs Args)
        {
            if (SomethingChanges)
            {
                var Result = MessageBox.Show("Would You Like To Save Changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Result == DialogResult.Yes && DeckInfo.Name != "N/A")
                {
                    using (var Writer = new BinaryWriter(File.Open(Save, FileMode.Open, FileAccess.Write)))
                    {
                        Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                        var DeckName = Encoding.BigEndianUnicode.GetBytes(textBox1.Text);
                        Writer.Write(DeckName);
                        Writer.Write(new byte[0x40 - Encoding.BigEndianUnicode.GetBytes(textBox1.Text).ToString().Length / 2]);
                    }
                }
            }
            base.OnClosing(Args);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SomethingChanges = true;
            }
        }
    }

    internal static class DeckInfo
    {
        public static string Name;
        public static int CardCount_Main;
        public static int CardCount_Extra;
        public static int CardCount_Side;
    }
}
