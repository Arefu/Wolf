using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Abaki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select File To Decode";
                Ofd.Filter = "Language file (*.bnd) | *.bnd";
                if (Ofd.ShowDialog() != DialogResult.OK) return;

                if (!Directory.Exists(new FileInfo(Ofd.FileName).Name))
                    Directory.CreateDirectory(new FileInfo(Ofd.FileName).Name);

                var LangFileName = new FileInfo(Ofd.FileName).Name;
                using (var Reader = new BinaryReader(File.Open(Ofd.FileName, FileMode.Open, FileAccess.Read)))
                {
                    const long DataStartOffset = 0x11CD;
                    const long AmountOfStrings = 0x472;
                    var ListOfStringOffSets = new List<int>();
                }
            }
        }

        private void ExportToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
        }
    }
}