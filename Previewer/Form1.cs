using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Previewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (!Directory.Exists("YGO_DATA"))
                MessageBox.Show("YGO_DATA Not Found! Some Things Might Be A Tad Broken.\nRefer To Wiki For More Info.", "YGO_DATA Missing!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void CardIndexToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var OFD = new OpenFileDialog())
            {
                OFD.Title = "Select Card Index File";
                OFD.Filter = "Card Index File (*.bin) | *.bin";

                if (OFD.ShowDialog() != DialogResult.OK)
                    return;

                if (!OFD.SafeFileName.StartsWith("CARD_Indx"))
                {
                    MessageBox.Show("Invalid File, Card Index Files Are Called \"CARD_Indx_#.bin\"", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void deckToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}