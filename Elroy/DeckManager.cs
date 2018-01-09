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

        public DeckManager(string DeckName, string SavePath)
        {
            InitializeComponent();
            Save = SavePath;
            DeckCode = Convert.ToInt32(DeckName.Replace("button", string.Empty)) * 0x130;

            label2.Text = GetDeckName();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists("YGO_DATA")) throw new Exception("Game Data Not Extracted!", new DirectoryNotFoundException("Can't Find YGO_DATA, Run Onomatoparia? Make Sure I'm In The Same Directory As YGO_DATA?"));
            if (!Directory.Exists("cardcropHD400.jpg.zib Unpacked") || !Directory.Exists("cardcropHD401.jpg.zib Unpacked")) throw new Exception("Card Images Not Extracted!", new DirectoryNotFoundException("Can't Find cardcropHD*** Folder, Run Relinquished? Make Sure I'm In The Same Directory As cardcropHD***?"));
            //Start Previewer.
        }

        private void ImportDeck_Click(object sender, EventArgs e)
        {
            var Result = MessageBox.Show("This WILL Overwrite Any Deck In This Slot, If You Want To Keep It Export The Deck First! Do You Wish To Continue?","Warning Overwriting Deck!",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (Result == DialogResult.No) return;

            using (var Writer = new BinaryWriter(File.Open(Save, FileMode.Open, FileAccess.Write)))
            {
                Writer.BaseStream.Position = DeckStartOffset + DeckCode;
                Writer.Write(new byte[0x130]);
            }
        }

        private void ExportDeck_Click(object sender, EventArgs e)
        {
            var DeckName = GetDeckName();
            if (string.IsNullOrEmpty(DeckName))
            {
                MessageBox.Show("I Can't Export An Empty Deck.", "Deck Is Empty!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            File.Create($"{DeckName}.ydc").Close(); //Ready For Importing To Save, AI importing requires a bit of handy work from me.
        }

        private string GetDeckName()
        {
            using (var Reader = new BinaryReader(File.Open(Save, FileMode.Open, FileAccess.Read)))
            {
                Reader.BaseStream.Position = DeckStartOffset + DeckCode;
                var DeckChunk = Reader.ReadBytes(0x130);
                var DeckName = Utilities.GetText(DeckChunk.Take(0x40).ToArray());
                return string.IsNullOrEmpty(DeckName) ? "N/A" : DeckName;
            }
        }
    }
}
