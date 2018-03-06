using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Wolf
{
    public partial class ModViewer : Form
    {
        public ModViewer()
        {
            InitializeComponent();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select Your MODDTA File";
                Ofd.Filter = "Wolf Mod Data |*.moddta";

                if (Ofd.ShowDialog() != DialogResult.OK) return;

                var ModData = new JavaScriptSerializer().Deserialize<ModInfo>(File.ReadAllText(Ofd.FileName));
                for (var Count = 0; Count < ModData.Files.Count; Count++)
                {
                    var CurrentFile = new ListViewItem(ModData.Files[Count]);
                    CurrentFile.SubItems.Add(Utilities.GiveFileSize(ModData.Sizes[Count]));

                    listView1.Items.Add(CurrentFile);
                }
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}