using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.Save_File;

namespace Elroy
{
    public partial class DeckManager : Form
    {
        private readonly int DeckPos;
        private readonly string Save;
        private bool SomethingChanges;

        public DeckManager(string DeckName, string SavePath)
        {
            InitializeComponent();
            var DeckManager = new Deck_Save_Database();
            using (var Reader = new BinaryReader(File.Open(SavePath, FileMode.Open, FileAccess.Read)))
            {
                DeckPos = (Game_Save.DecksOffset - 0x130) +
                          0x130 * Convert.ToInt32(DeckName.Replace("button", string.Empty));
                Reader.BaseStream.Position = DeckPos; //Terrible
                DeckManager.LoadDeckData(Reader);

                label4.Text = $@"{DeckManager.MainDeckCards.Count} - {DeckManager.SideDeckCards.Count} - {DeckManager.ExtraDeckCards.Count}";
                textBox1.Text = DeckManager.DeckName;
                textBox1.MaxLength = Constants.DeckNameLen;
                Reader.Close();
            }

            Save = SavePath;
        }

        private void ImportDeck_Click(object Sender, EventArgs Args)
        {
            var Result =
                MessageBox.Show("This WILL Overwrite Any Deck In This Slot, If You Want To Keep It Export The Deck First! Do You Wish To Continue?", "Warning Overwriting Deck!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.No) return;

            Export_Deck.Enabled = true;
            textBox1.Text = "N/A";

            using (var Writer = new BinaryWriter(File.Open(Save, FileMode.Open, FileAccess.Write)))
            {
                Writer.BaseStream.Position = DeckPos;
                // Writer.Write(new byte[0x130]);

                Writer.Close();
            }
        }

        private void ExportDeck_Click(object Sender, EventArgs Args)
        {
            if (checkBox2.Checked) //Export AI compatible deck
            {
                using (var Reader = new BinaryReader(File.Open(Save, FileMode.Open, FileAccess.Read)))
                {
                    using (var Writer = new BinaryWriter(File.Open($"{textBox1.Text}.ydc", FileMode.OpenOrCreate, FileAccess.Write)))
                    {
                        Reader.BaseStream.Position = DeckPos + Constants.DeckIndexYdcStart*2;
                        Writer.Write(Reader.ReadBytes(0xF0));
                    }
                }
            }

        MessageBox.Show("Export Complete!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        if (((TextBox)Sender).ContainsFocus) SomethingChanges = true;
    }
}
}