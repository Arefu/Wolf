using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Wolf
{
    public partial class Form1 : Form
    {
        public static string InstallDir;
        public static StreamReader Reader;
        public static List<FileData> Data = new List<FileData>();

        private TreeNode _endNode; //Recursive Func, Needs To Be Outside.

        public Form1()
        {
            InitializeComponent();
            FileQuickViewList.ImageList = NodeImages;
            MainFileView.SmallImageList = NodeImages;
        }

        private void Form1_Load(object Sender, EventArgs Args)
        {
            GameLocLabel.ForeColor = Color.Red;
            GameLocLabel.Text = "Game Not Loaded";
        }

        private void ExitToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            if (FileQuickViewList.Nodes.Count != 0) return;

            Reader?.Close();
            try
            {
                InstallDir = Utilities.GetInstallDir();
                Reader = new StreamReader(File.Open($"{InstallDir}\\YGO_DATA.TOC", FileMode.Open, FileAccess.Read));
            }
            catch
            {
                var Reply = MessageBox.Show(this, "Do You Want To Locate Game?", "Game Not Fuond!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (Reply == DialogResult.No) Environment.Exit(1);
                else
                    using (var Ofd = new OpenFileDialog())
                    {
                        Ofd.Title = "Select YuGiOh.exe";
                        Ofd.Filter = "YuGiOh.exe | YuGiOh.exe";
                        var Result = Ofd.ShowDialog();
                        if (Result != DialogResult.OK) Environment.Exit(1);
                        Reader = new StreamReader(File.Open($"{new FileInfo(Ofd.FileName).DirectoryName}\\YGO_DATA.TOC",
                            FileMode.Open, FileAccess.Read));
                        InstallDir = new FileInfo(Ofd.FileName).DirectoryName;
                    }
            }

            Reader.ReadLine();

            GameLocLabel.ForeColor = Color.Green;
            GameLocLabel.Text = "Game Loaded";

            var RootNode = new TreeNode("YGO_DATA");
            FileQuickViewList.Nodes.Add(RootNode);
            while (!Reader.EndOfStream)
            {
                var Line = Reader.ReadLine();
                if (Line == null) continue;
                Line = Line.TrimStart(' ');
                Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled);
                var LineData = Line.Split(' ');
                Data.Add(new FileData(Utilities.HexToDec(LineData[0]), Utilities.HexToDec(LineData[1]), LineData[2]));
                LineData[2].Split('\\').Aggregate(RootNode,
                    (Current, File) => Current.Nodes.ContainsKey(File)
                        ? Current.Nodes[File]
                        : Current.Nodes.Add(File, File));
            }

            Reader?.Close();
            GiveIcons(FileQuickViewList.Nodes[0]);
            FileQuickViewList.Nodes[0].Expand();
            FileQuickViewList.SelectedNode = FileQuickViewList.Nodes[0];
            FileQuickViewList_NodeMouseClick(new object(),
                new TreeNodeMouseClickEventArgs(FileQuickViewList.Nodes[0], MouseButtons.Left, 1, 0, 0));
        }

        private void CloseToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            MainFileView.Items.Clear();
            FileQuickViewList.Nodes.Clear();
            Reader?.Close();
            GameLocLabel.ForeColor = Color.Red;
            GameLocLabel.Text = "Game Not Loaded";
        }

        private void FileQuickViewList_NodeMouseClick(object Sender, TreeNodeMouseClickEventArgs Args)
        {
            if (Args.Node.Nodes.Count <= 0) return;

            MainFileView.Items.Clear();
            MainFileView.LargeImageList = NodeImages;

            for (var Node = 0; Node < Args.Node.Nodes.Count; Node++)
            {
                var Items = new ListViewItem(Args.Node.Nodes[Node].Name);
                var FileSizeObject = Data.Where(Item => Item.Item3.Contains(Args.Node.Nodes[Node].Text))
                    .Select(NodeSize => NodeSize.Item1).FirstOrDefault();


                MainFileView.Items.Add(Items);
                MainFileView.Items[Node].ImageIndex = Args.Node.Nodes[Node].Nodes.Count == 0 ? 1 : 0;
                if (Args.Node.Nodes[Node].Name.EndsWith(".png") || Args.Node.Nodes[Node].Name.EndsWith(".jpg"))
                    MainFileView.Items[Node].ImageIndex = 2;

                Items.SubItems.Add(Items.ImageIndex != 0 ? Utilities.GiveFileSize(FileSizeObject) : "Directory");
            }

            MainFileView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            MainFileView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void MainFileView_MouseDoubleClick(object Sender, MouseEventArgs Args)
        {
            var SelectMe = GetNode(FileQuickViewList.Nodes[0]);

            SelectMe.Expand();
            FileQuickViewList.SelectedNode = SelectMe;
            FileQuickViewList_NodeMouseClick(new object(), new TreeNodeMouseClickEventArgs(SelectMe, MouseButtons.Left, 1, 0, 0));
        }

        private TreeNode GetNode(TreeNode CurrentNode)
        {
            foreach (TreeNode Node in CurrentNode.Nodes)
            {
                if (Node.Text == MainFileView.SelectedItems[0].Text)
                {
                    _endNode = Node;
                    return Node;
                }

                GetNode(Node);
            }

            return _endNode;
        }

        private static void GiveIcons(TreeNode RootNode)
        {
            foreach (TreeNode Node in RootNode.Nodes)
            {
                if (Node.Nodes.Count != 0)
                {
                    Node.ImageIndex = 0;
                    Node.SelectedImageIndex = 0;
                }
                else if (Node.Text.EndsWith("jpg") || Node.Text.EndsWith("png"))
                {
                    Node.SelectedImageIndex = 2;
                    Node.ImageIndex = 2;
                }
                else
                {
                    Node.ImageIndex = 1;
                    Node.SelectedImageIndex = 1;
                }

                GiveIcons(Node);
            }
        }

        private void MainFileView_MouseClick(object Sender, MouseEventArgs Args)
        {
            if (Args.Button != MouseButtons.Right) return;
            FileHandleMenu.Show(MousePosition);
        }

        private void FileHandleMenu_ItemClicked(object Sender, ToolStripItemClickedEventArgs Args)
        {
            switch (Args.ClickedItem.Text)
            {
                case "Extract":
                    if (MainFileView.SelectedItems[0].ImageIndex != 0)
                        ContextMenuFunctions.ExtractFile(MainFileView.SelectedItems[0]);
                    else
                        MessageBox.Show(this, "This Build Can't Extract Whole Folders, Use Onomatoparia",
                            "Can't Extract Folders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "View":
                    if (MainFileView.SelectedItems[0].ImageIndex == 2)
                        ContextMenuFunctions.ViewImage(MainFileView.SelectedItems[0]);
                    else
                        MessageBox.Show(this, "You Can't View This File Type", "Can't View File/Folder",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        public class FileData
        {
            public FileData(int Item1, int Item2, string Item3)
            {
                this.Item1 = Item1;
                this.Item2 = Item2;
                this.Item3 = Item3;
            }

            public int Item1 { get; set; }
            public int Item2 { get; set; }
            public string Item3 { get; set; }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ModerUi = new ModUI(); //Might Make Static
            ModerUi.Show();
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var ModView = new ModViewer();
            ModView.Show();
        }
    }
}