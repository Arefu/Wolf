using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class CardArtEditor : Form
    {
        ///The designer threw up when I tried to assign the ImageLists declaratively
        ///so we are doing it yolo style.
        private readonly ImageList _largeImageList;
        private readonly ImageList _smallImageList;
        private readonly BlueEyesLogic _blueEyesLogic;

        public CardArtEditor()
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
        }

        public void AddObjectsToObjectListView(List<Artwork> artworkList)
        {
            fastObjectListView1.AddObjects(artworkList);
        }

        private void SetupImageList()
        {
            _largeImageList.ColorDepth = ColorDepth.Depth24Bit;
            _largeImageList.ImageSize = new Size(256, 256);
            fastObjectListView1.LargeImageList = _largeImageList;

            _smallImageList.ColorDepth = ColorDepth.Depth24Bit;
            _smallImageList.ImageSize = new Size(256, 256);
            fastObjectListView1.SmallImageList = _smallImageList;
        }

        private void SetupColumns()
        {
            //Apparently something other than the image must be shown in the coloumn for the image to be visible
            GI.AspectGetter = x => ((Artwork) x).GameImageFilePath;
            //The image that actually will be shown instead of the above path string
            GI.ImageGetter = GameImageGetter;
            GIFileName.AspectGetter = x => ((Artwork) x).GameImageFileName;
            GICardName.AspectGetter = x => ((Artwork) x).GameImageMonsterName;

            RI.AspectGetter = x => ((Artwork)x).ReplacementImageFilePath;
            RI.ImageGetter = ReplacementImageGetter;
            RICardName.AspectGetter = x => ((Artwork)x).ReplacementImageMonsterName;
            RIFileName.AspectGetter = x => ((Artwork)x).ReplacementImageFileName;
        }

        private void FilterRows()
        {
            var filter = TextMatchFilter.Contains(fastObjectListView1, txt_search.Text);
            if (fastObjectListView1.DefaultRenderer == null)
            {
                fastObjectListView1.DefaultRenderer = new HighlightTextRenderer(filter);
            }

            fastObjectListView1.AdditionalFilter = filter;
        }

        private object GameImageGetter(object row)
        {
            var artworkRow = ((Artwork)row);
            var gameImagePath = artworkRow.GameImageFilePath;

            if (string.IsNullOrEmpty(gameImagePath))
            {
                throw new ArgumentNullException($"Can not load GameImageGetter for: {artworkRow}");
            }

            if (!fastObjectListView1.LargeImageList.Images.ContainsKey(gameImagePath))
            {
                UpdateImageLists(gameImagePath);
            }
            return gameImagePath;
        }

        private object ReplacementImageGetter(object row)
        {
            var artworkRow = ((Artwork)row);
            var replacementImagePath = artworkRow.ReplacementImageFilePath;

            if (string.IsNullOrEmpty(replacementImagePath))
            {
                throw new ArgumentNullException($"Can not load ReplacementImageGetter for: {artworkRow}");
            }

            if (!fastObjectListView1.LargeImageList.Images.ContainsKey(replacementImagePath))
            {
                UpdateImageLists(replacementImagePath);
            }
            return replacementImagePath;
        }

        private void UpdateImageLists(string imagePath)
        {
            //Both lists must be in sync to use anything other than the DETAILS view. Yes I know
            //that is what we are using, but if we use another view in the future we most likely won't remember this.
            Image smallImage = Image.FromFile(imagePath);
            Image largeImage = Image.FromFile(imagePath);
            fastObjectListView1.SmallImageList.Images.Add(imagePath, smallImage);
            fastObjectListView1.LargeImageList.Images.Add(imagePath, largeImage);

            Debug.WriteLine($"Image: {imagePath} is about to be shown");
            Debug.WriteLine($"SmallImageList count: {fastObjectListView1.SmallImageList.Images.Count}");
            Debug.WriteLine($"LargeImageList count: {fastObjectListView1.LargeImageList.Images.Count}");
        }

        private void btn_run_Click(object sender, System.EventArgs e)
        {
            _blueEyesLogic.Run();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            FilterRows();
        }
    }
}
