using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Previewer
{
    public partial class Form1 : Form
    {
        private readonly Manager Man = new Manager();
        private Card_Manager Card;
        private short GlobalIndex = 3900;
        private Localized_Text.Language Language = Localized_Text.Language.English;

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
            cardTitle.Text = Card.Cards[GlobalIndex].Name.GetText(Language);
            cardDesc.Text = Card.Cards[GlobalIndex].Description.GetText(Language);

            if (Directory.Exists("cardcropHD400.jpg.zib Unpacked"))
            {
                var ImageFile = Path.Combine(Path.Combine(Application.StartupPath, "cardcropHD400.jpg.zib Unpacked"),
                    $"{Card.Cards[GlobalIndex].CardId.ToString()}.jpg");
                if (File.Exists(ImageFile))
                {
                    pictureBox1.Image = Image.FromFile(ImageFile);
                    radioButton1.Checked = false;
                }
                else if (Directory.Exists("cardcropHD401.jpg.zib Unpacked"))
                {
                    ImageFile = Path.Combine(Path.Combine(Application.StartupPath, "cardcropHD401.jpg.zib Unpacked"),
                        $"{Card.Cards[GlobalIndex].CardId.ToString()}.jpg");

                    if (File.Exists(ImageFile))
                    {
                        pictureBox1.Image = Image.FromFile(ImageFile);
                        radioButton1.Checked = true;
                    }
                }
            }

            AtkTextBox.Text = Card.Cards[GlobalIndex].Atk.ToString();
            DefTextBox.Text = Card.Cards[GlobalIndex].Def.ToString();
            textBox1.Text = Card.Cards[GlobalIndex].CardId.ToString();
            textBox2.Text = Card.Cards[GlobalIndex].CardType.ToString();
            textBox3.Text = Card.Cards[GlobalIndex].Attribute.ToString();
            textBox4.Text = Card.Cards[GlobalIndex].Level.ToString();
            textBox5.Text = Card.Cards[GlobalIndex].Limit.ToString();
            numericUpDown1.Value = GlobalIndex;
        }

        private void DeckToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
        }

        private void LanguageToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            foreach (ToolStripMenuItem Lang in languageToolStripMenuItem.DropDownItems) Lang.Checked = false;

            ((ToolStripMenuItem) Sender).Checked = true;
            switch (((ToolStripMenuItem) Sender).Text)
            {
                case "English":
                    Language = Localized_Text.Language.English;
                    break;
                case "French":
                    Language = Localized_Text.Language.French;
                    break;
                case "German":
                    Language = Localized_Text.Language.German;
                    break;
                case "Italian":
                    Language = Localized_Text.Language.Italian;
                    break;
                case "Spanish":
                    Language = Localized_Text.Language.Spanish;
                    break;
                default:
                    Language = Localized_Text.Language.Unknown;
                    break;
            }
        }

        private void Button1_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex <= Card.Cards.Keys.Max())
            {
                GlobalIndex++;
                GetNextIndex();
                LoadCards();
            }
            else
            {
                MessageBox.Show(@"You're At The Last Card", @"No More Cards!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void GetNextIndex(bool Forward = true)
        {
            if (Forward)
            {
                if (Card.Cards.ContainsKey(GlobalIndex))
                    return;
                do
                {
                    GlobalIndex++;
                } while (!Card.Cards.ContainsKey(GlobalIndex));
            }
            else
            {
                if (Card.Cards.ContainsKey(GlobalIndex))
                    return;
                do
                {
                    GlobalIndex--;
                } while (!Card.Cards.ContainsKey(GlobalIndex));
            }
        }

        private void PrevCard_Click(object Sender, EventArgs Args)
        {
            if (GlobalIndex > 3900)
            {
                GlobalIndex--;
                GetNextIndex(false);
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
            GlobalIndex = (short) numericUpDown1.Value;
            LoadCards();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Soon™");
        }
    }
}