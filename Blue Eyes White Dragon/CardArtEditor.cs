using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Blue_Eyes_White_Dragon
{
    public partial class CardArtEditor : Form
    {
        public const string GameCardsLocation = "GameCardsLocation";
        ///The designer threw up when I tried to assign the ImageLists declaratively
        ///so we are doing it yolo style.
        readonly ImageList LargeImageList = new ImageList();
        readonly ImageList SmallImageList = new ImageList();

        public CardArtEditor()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var gameCardsLocationSetting = ConfigurationManager.AppSettings[GameCardsLocation];
            var gameCardsLocation = new DirectoryInfo(gameCardsLocationSetting);

            var artworkList = CreateArtworkList(gameCardsLocation);

            SetupImageList();
            SetupColumns();

            fastObjectListView1.AddObjects(artworkList);
        }

        private void SetupImageList()
        {
            LargeImageList.ColorDepth = ColorDepth.Depth24Bit;
            LargeImageList.ImageSize = new Size(256, 256);
            fastObjectListView1.LargeImageList = LargeImageList;

            SmallImageList.ColorDepth = ColorDepth.Depth24Bit;
            SmallImageList.ImageSize = new Size(256, 256);
            fastObjectListView1.SmallImageList = SmallImageList;
        }

        private void SetupColumns()
        {
            //Apparently something other than the image must be shown in the coloumn for the image to be visible
            GameImage.AspectGetter = delegate (object x) { return ((ArtworkModel)x).GameImagePath; };
            //The image that actually will be shown instead of the above path string
            GameImage.ImageGetter = delegate (object row) {
                ArtworkModel artworkRow = ((ArtworkModel)row);
                var gameImagePath = artworkRow.GameImagePath;

                if (!fastObjectListView1.LargeImageList.Images.ContainsKey(gameImagePath))
                {
                    //Both lists must be in sync to use anything other than the DETAILS view. Yes I know
                    //that is what we are using, but if we use another view in the future we most likely won't remember this.
                    Image smallImage = Image.FromFile(gameImagePath);
                    Image largeImage = Image.FromFile(gameImagePath);
                    fastObjectListView1.SmallImageList.Images.Add(gameImagePath, smallImage);
                    fastObjectListView1.LargeImageList.Images.Add(gameImagePath, largeImage);
                }
                Debug.WriteLine($"Image: {gameImagePath} is about to be shown");
                Debug.WriteLine($"SmallImageList count: {fastObjectListView1.SmallImageList.Images.Count}");
                Debug.WriteLine($"LargeImageList count: {fastObjectListView1.LargeImageList.Images.Count}");
                return gameImagePath;
            };
        }

        private List<ArtworkModel> CreateArtworkList(DirectoryInfo dir)
        {
            var files = Directory
                .EnumerateFiles(dir.FullName)
                .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png"))
                .Select(x => new FileInfo(x))
                .ToList();

            Debug.WriteLine($"Number of images found in {dir.FullName}: {files.Count}");

            var artworkList = new List<ArtworkModel>();

            foreach (var file in files)
            {
                artworkList.Add(new ArtworkModel()
                {
                    GameImagePath = file.FullName,
                    GameImageFormat = file.Extension
                });
            }
            return artworkList;
        }
    }
}
