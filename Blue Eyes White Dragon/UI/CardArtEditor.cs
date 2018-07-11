using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.BusinessLogic;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.UI_Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class CardArtEditor : Form
    {
        public const string GameImagesLocation = "GameImagesLocation";
        public const string ReplacementImagesLocation = "ReplacementImagesLocation";

        ///The designer threw up when I tried to assign the ImageLists declaratively
        ///so we are doing it yolo style.
        private readonly ImageList _largeImageList;
        private readonly ImageList _smallImageList;
        private readonly CardDbContext _db;
        private readonly ArtworkManager _cardManager;

        public CardArtEditor()
        {
            InitializeComponent();
            _db = new CardDbContext();
            _largeImageList = new ImageList();
            _smallImageList = new ImageList();
            _cardManager = new ArtworkManager(_db);
            Init();
        }

        private void Init()
        {
            var gameImagesLocation = FileLoader.LoadCardDir(GameImagesLocation);
            var replacementImagesLocation = FileLoader.LoadCardDir(ReplacementImagesLocation);
            var artworkList = _cardManager.CreateArtworkList(gameImagesLocation, replacementImagesLocation);
            SetupImageList();
            SetupColumns();

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
            GI.AspectGetter = x => ((ArtworkModel) x).GameImagePath;
            //The image that actually will be shown instead of the above path string
            GI.ImageGetter = new ImageGetterDelegate(GameImageGetter);
            GIFileName.AspectGetter = x => ((ArtworkModel) x).GameImageFileName;

            RI.AspectGetter = x => ((ArtworkModel)x).ReplacementImagePath;
            RI.ImageGetter = new ImageGetterDelegate(ReplacementImageGetter);

            RICardName.AspectGetter = x => ((ArtworkModel)x).ReplacementImageMonsterName;
            RIFileName.AspectGetter = x => ((ArtworkModel)x).ReplacementImageFileName;
        }

        private object GameImageGetter(object row)
        {
            var artworkRow = ((ArtworkModel)row);
            var gameImagePath = artworkRow.GameImagePath;

            if (!fastObjectListView1.LargeImageList.Images.ContainsKey(gameImagePath))
            {
                UpdateImageLists(gameImagePath);
            }
            return gameImagePath;
        }

        private object ReplacementImageGetter(object row)
        {
            var artworkRow = ((ArtworkModel)row);
            var replacementImagePath = artworkRow.ReplacementImagePath;

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
    }
}
