using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class CardArtEditor : Form, IArtworkEditor
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
            GI.ImageGetter = _blueEyesLogic.GameImageGetter;
            GIFileName.AspectGetter = x => ((Artwork) x).GameImageFileName;
            GICardName.AspectGetter = x => ((Artwork) x).GameImageMonsterName;

            RI.AspectGetter = x => ((Artwork)x).ReplacementImageFilePath;
            RI.ImageGetter = _blueEyesLogic.ReplacementImageGetter;
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

        private void Btn_run_Click(object sender, System.EventArgs e)
        {
            _blueEyesLogic.Run();
        }

        private void Txt_search_TextChanged(object sender, EventArgs e)
        {
            FilterRows();
        }

        public bool LargeImageListContains(string gameImagePath)
        {
            return fastObjectListView1.LargeImageList.Images.ContainsKey(gameImagePath);
        }

        public void SmallImageListAdd(string imagePath, Image smallImage)
        {
            fastObjectListView1.SmallImageList.Images.Add(imagePath, smallImage);
        }

        public void LargeImagelistAdd(string imagePath, Image largeImage)
        {
            fastObjectListView1.LargeImageList.Images.Add(imagePath, largeImage);
        }

        public int SmallImageListGetCount()
        {
            return fastObjectListView1.SmallImageList.Images.Count;
        }

        public int LargeImageListGetCount()
        {
            return fastObjectListView1.LargeImageList.Images.Count;
        }
    }
}
