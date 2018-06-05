using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Previewer
{
    public partial class Form1 : Form
    {
        private char Language = (char)CurrentLanguage.English;
        private enum CurrentLanguage
        {
            English = 'E',
            Spanish = 'S',
            German = 'G',
            Italian = 'I',
            Unkown = '?'
        }; //Default To English.
        private string CurrentUnpackedImagesDir = string.Empty;
        private short GlobalIndex = 3900;
        private Manager Man = new Manager();
        private Card_Manager Card;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object Sender, EventArgs Args)
        {
            //if (Directory.Exists("YGO_DATA")) return;
            // MessageBox.Show(@"YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", @"YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Application.Exit();
        }

        private void ExitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Application.Exit();
        }

        private void CardIndexToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Card = new Card_Manager(Man);
            Man.Load();
            Card.Load();

            LoadCards();
        }

        private void LoadCards()
        {
            cardTitle.Text = Card.Cards[GlobalIndex].Name.GetText(Localized_Text.Language.English); //TODO: Replace With Proper Language Checking
            cardDesc.Text = Card.Cards[GlobalIndex].Description.GetText(Localized_Text.Language.English); //TODO: Replace With Proper Language Checking

            if (Directory.Exists("cardcropHD400.jpg.zib Unpacked"))
            {
                var ImageFile = Path.Combine(Path.Combine(Application.StartupPath, "cardcropHD400.jpg.zib Unpacked"), $"{Card.Cards[GlobalIndex].CardId.ToString()}.jpg");
                if (File.Exists(ImageFile))
                {
                    pictureBox1.Image = Image.FromFile(ImageFile);
                }
            }

            AtkTextBox.Text = Card.Cards[GlobalIndex].Atk.ToString();
            DefTextBox.Text = Card.Cards[GlobalIndex].Def.ToString();
            textBox1.Text = Card.Cards[GlobalIndex].CardId.ToString();
            textBox2.Text = Card.Cards[GlobalIndex].CardType.ToString();
            textBox3.Text = Card.Cards[GlobalIndex].Attribute.ToString();
            numericUpDown1.Value = GlobalIndex;
        }

        private void DeckToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
        }

        private void LanguageToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            foreach (ToolStripMenuItem Language in languageToolStripMenuItem.DropDownItems)
                Language.Checked = false;

            ((ToolStripMenuItem)Sender).Checked = true;
            Language = ((ToolStripMenuItem)Sender).Text[0];
        }

        private void Button1_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex <= Card.Cards.Count)
            {
                GlobalIndex++;
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Last Card", @"No More Cards!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void GetNextIndex()
        {

        }

        private void PrevCard_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex > 3900)
            {
                GlobalIndex--;
                numericUpDown1.Value--;
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Fist Card", @"No More Cards!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void Button1_Click_1(object Sender, EventArgs Args)
        {
            GlobalIndex = (short)numericUpDown1.Value;
            LoadCards();
        }

        private void CheckBox1_CheckedChanged(object Sender, EventArgs Args)
        {
            checkBox1.Checked = false;
        }

        private struct CardTemplate
        {
            internal uint Atk;
            internal uint Def;
            internal uint Level;
            internal short Id;
        }
    }
}