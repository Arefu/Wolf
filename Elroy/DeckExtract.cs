using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Elroy
{
    public partial class DeckExtract : Form
    {
        public DeckExtract()
        {
            InitializeComponent();
        }

        public string DeckToExport { get; set; }

        private void label1_Click(object Sender, EventArgs Args)
        {
            Process.Start("https://github.com/Arefu/Wolf/wiki");
        }

        private void button1_Click(object Sender, EventArgs Args)
        {
            DeckToExport = "Save";
            Close();
        }

        private void button2_Click(object Sender, EventArgs Args)
        {
            DeckToExport = "AI";
            Close();
        }
    }
}