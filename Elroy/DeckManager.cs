using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;
using Yu_Gi_Oh.Save_File;

namespace Elroy
{
    public partial class DeckManager : Form
    {
        private const long DeckStartOffset = 0x2C80; //0x2DB0 Should Be The First Deck;
        private readonly int DeckCode;
        private readonly string Save;
        private bool SomethingChanges;

        public DeckManager(string DeckName, string SavePath)
        {
            InitializeComponent();
            var DeckManager = new Deck_Save_Database();
            using (var Reader = new BinaryReader(File.Open(SavePath,FileMode.Open,FileAccess.Read)))
            {
                var DeckNumberOffset = Convert.ToInt32(DeckName.Replace("button", string.Empty));
                Reader.BaseStream.Position = Game_Save.DecksOffset;
                DeckManager.LoadDeckData(Reader);

                label4.Text = $@"{DeckManager.MainDeckCards.Count} - {DeckManager.SideDeckCards.Count} -{DeckManager.ExtraDeckCards.Count}";
                textBox1.Text = DeckManager.DeckName;
                textBox1.MaxLength = Yu_Gi_Oh.File_Handling.Utility.Constants.DeckNameLen;
            }
            Save = SavePath;

            GetDeckInfo();
        }

        private void ImportDeck_Click(object Sender, EventArgs Args)
        {
            var Result =
                MessageBox.Show(
                    "This WILL Overwrite Any Deck In This Slot, If You Want To Keep It Export The Deck First! Do You Wish To Continue?",
                    "Warning Overwriting Deck!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.No) return;

            Export_Deck.Enabled = true;
            textBox1.Text = "N/A";

            using (var Writer = new BinaryWriter(File.Open(Save, FileMode.Open, FileAccess.Write)))
            {
                Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                Writer.Write(new byte[0x130]);

                //Re-Write. (Can Only Import Save Style Decks So Far?
                //using (var OFD = new OpenFileDialog())
                //{
                //    OFD.Title = "Select Your YDC Deck File";
                //    OFD.Filter = "Deck file (*.ydc) | *.ydc";
                //    if (OFD.ShowDialog() != DialogResult.OK) return;
                //    if (File.ReadAllBytes(OFD.FileName).Length > 0xC0) throw new IOException("YDC File To Big!");

                //    Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                //    Writer.Write(Encoding.BigEndianUnicode.GetBytes(new FileInfo(OFD.FileName).Name));
                //    Writer.Write(new byte[0x30 - Encoding.BigEndianUnicode.GetBytes(new FileInfo(OFD.FileName).Name).Skip(1).ToArray().Length / 2]);
                //    Writer.Write(File.ReadAllBytes(OFD.FileName));
                //}

                Writer.Close();
            }
        }

        private void ExportDeck_Click(object Sender, EventArgs Args)
        {
           
            MessageBox.Show("Export Complete!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GetDeckInfo()
        {
            
        }

        protected override void OnClosing(CancelEventArgs Args)
        {
            if (!SomethingChanges)
            {
                base.OnClosing(Args);
                return;
            }

          
            base.OnClosing(Args);
        }

        private void textBox1_TextChanged(object Sender, EventArgs Args)
        {
            if (((TextBox) Sender).ContainsFocus) SomethingChanges = true;
        }
    }
}