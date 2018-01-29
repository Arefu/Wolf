using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Wolf
{
    public partial class ModUI : Form
    {
        public ModUI()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var OFD = new OpenFileDialog())
            {
                OFD.Title = "Select Files To Add To Mod...";
                OFD.Multiselect = true;
                if (OFD.ShowDialog() != DialogResult.OK) return;

                foreach (var Item in OFD.FileNames)
                {
                    var File = new FileInfo(Item);
                    if (File.Directory != null && !File.Directory.FullName.Contains("YGO_DATA"))
                    {
                        MessageBox.Show("YGO_DATA Structure Breach: Refer to Wiki for more Information (Link in clipboard)", "YGO_DATA Structure Breach!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Clipboard.SetText("https://github.com/Arefu/Wolf/wiki/YGO_DATA-Structure-Breach");
                        return;
                    }

                    listBox1.Items.Add($"YGO_DATA{File.FullName.Split(new[] { "YGO_DATA" }, StringSplitOptions.None)[1]}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count <= 0 || listBox1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("There Are No Items To Remove", "No Items To Remove", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            var Result = MessageBox.Show("Are You Sure You Want To REMOVE These Files From The Mod?", "Confirm Removal From Mod", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (Result != DialogResult.Yes) return;

            for (var Count = 0; Count < listBox1.SelectedItems.Count; Count++)
            {
                listBox1.Items.RemoveAt(Count);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count <= 0)
            {
                MessageBox.Show("There Are No Items In The Mod!", "No Changed Files To Pack!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox1.Text == "Mod Name..." && textBox1.ForeColor != Color.Black)
            {
                MessageBox.Show("Please Name Your Mod!", "Mod Doesn't Have A Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var Archive = ZipFile.Open($"{textBox1.Text}.modpkg", ZipArchiveMode.Update)) //Shhh It's just a ZIP
            {
                var FileInfo = new ModInfo();
                foreach (var Item in listBox1.Items)
                {
                    var Info = new FileInfo(Item.ToString());
                    FileInfo.Files.Add(Info.FullName.Split(new[] { "YGO_DATA\\" }, StringSplitOptions.None)[1]);
                    FileInfo.Sizes.Add(Info.Length);
                    Archive.CreateEntryFromFile(Info.FullName, Info.Name);
                }

                File.WriteAllText($"{Application.StartupPath}\\{textBox1.Text}.moddta", new JavaScriptSerializer().Serialize(FileInfo));
            }

            MessageBox.Show("Mod Packing Complete!", "Finished Packing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text != "Mod Name...") return;

            textBox1.Clear();
            textBox1.ForeColor = Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) return;

            textBox1.Text = "Mod Name...";
            textBox1.ForeColor = Color.Gray;
        }
    }
}
