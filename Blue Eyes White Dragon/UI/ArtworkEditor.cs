using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class ArtworkEditor : Form, IArtworkEditor
    {
        ///The designer threw up when I tried to assign the ImageLists declaratively
        ///so we are doing it yolo style.
        private readonly ImageList _largeImageList;
        private readonly ImageList _smallImageList;
        private readonly IBlueEyesLogic _blueEyesLogic;

        public ArtworkEditor()
        {
            InitializeComponent();
            _largeImageList = new ImageList();
            _smallImageList = new ImageList();
            _blueEyesLogic = new BlueEyesLogic(this);

            Init();
        }

        private void Init()
        {
            SetupImageList();
            SetupColumns();
            LoadSettings();
        }

        private void LoadSettings()
        {
            txt_card_match_path.Text = Properties.Settings.Default.LastUsedLoadPath;
        }

        public void AddObjectsToObjectListView(List<Artwork> artworkList)
        {
            objlist_artwork_editor.AddObjects(artworkList);
        }

        private void SetupImageList()
        {
            _largeImageList.ColorDepth = ColorDepth.Depth24Bit;
            _largeImageList.ImageSize = new Size(256, 256);
            objlist_artwork_editor.LargeImageList = _largeImageList;

            _smallImageList.ColorDepth = ColorDepth.Depth24Bit;
            _smallImageList.ImageSize = new Size(256, 256);
            objlist_artwork_editor.SmallImageList = _smallImageList;
        }

        private void SetupColumns()
        {
            //Apparently something other than the image must be shown in the coloumn for the image to be visible
            GI.AspectGetter = x => ((Artwork) x).GameImageFilePath;
            //The image that actually will be shown instead of the above path string
            GI.ImageGetter = _blueEyesLogic.GameImageGetter;
            GIFileName.AspectGetter = x => ((Artwork) x).GameImageFileName;
            GICardName.AspectGetter = x => ((Artwork) x).GameImageMonsterName;

            GIWidth.AspectGetter = x => ((Artwork)x).GameImageWidth;
            GIHeight.AspectGetter = x => ((Artwork)x).GameImageHeight;

            RI.AspectGetter = x => ((Artwork)x).ReplacementImageFilePath;
            RI.ImageGetter = _blueEyesLogic.ReplacementImageGetter;
            RICardName.AspectGetter = x => ((Artwork)x).ReplacementImageMonsterName;
            RIFileName.AspectGetter = x => ((Artwork)x).ReplacementImageFileName;

            RIWidth.AspectGetter = x => ((Artwork) x).ReplacementImageWidth;
            RIHeight.AspectGetter = x => ((Artwork) x).ReplacementImageHeight;
        }

        private void FilterRows()
        {
            var filter = TextMatchFilter.Contains(objlist_artwork_editor, txt_search.Text);

            if (objlist_artwork_editor.DefaultRenderer == null)
            {
                objlist_artwork_editor.DefaultRenderer = new HighlightTextRenderer(filter);
            }

            objlist_artwork_editor.AdditionalFilter = filter;
        }

        private void Btn_run_Click(object sender, System.EventArgs e)
        {
            _blueEyesLogic.RunMatchAll();
        }

        private void Txt_search_TextChanged(object sender, EventArgs e)
        {
            FilterRows();
        }

        public bool LargeImageListContains(string gameImagePath)
        {
            return objlist_artwork_editor.LargeImageList.Images.ContainsKey(gameImagePath);
        }

        public void SmallImageListAdd(string imagePath, Image smallImage)
        {
            objlist_artwork_editor.SmallImageList.Images.Add(imagePath, smallImage);
        }

        public int GetConsoleLineNumber()
        {
            return richtextbox_console.Lines.Length;
        }

        public void AppendConsoleText(string message)
        {
            richtextbox_console.AppendText(message);
        }

        public void RemoveOldestLine()
        {
            var lines = richtextbox_console.Lines.ToList();
            lines.RemoveAt(0);
            richtextbox_console.Lines = lines.ToArray();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        private void Btn_save_match_Click(object sender, EventArgs e)
        {
            _blueEyesLogic.RunSaveMatch();
        }
        private void Btn_load_match_Click(object sender, EventArgs e)
        {
            _blueEyesLogic.RunLoadMatch(txt_card_match_path.Text);
        }

        private void Richtextbox_console_TextChanged(object sender, EventArgs e)
        {
            richtextbox_console.SelectionStart = richtextbox_console.TextLength;
            richtextbox_console.ScrollToCaret();
        }

        private void Btn_browse_match_file_path_Click(object sender, EventArgs e)
        {
            if (open_file_browse_match_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_card_match_path.Text = open_file_browse_match_file.FileName;
                Properties.Settings.Default.LastUsedLoadPath = txt_card_match_path.Text;
                Properties.Settings.Default.Save();
            }
        }
    }
}
