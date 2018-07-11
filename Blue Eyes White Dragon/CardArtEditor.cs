using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon
{
    public partial class CardArtEditor : Form
    {
        public const string GameImagesLocation = "GameImagesLocation";
        public const string ReplacementImagesLocation = "ReplacementImagesLocation";
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
            var artworkList = LoadCardDirs();

            SetupImageList();
            SetupColumns();

            fastObjectListView1.AddObjects(artworkList);
        }

        private List<ArtworkModel> LoadCardDirs()
        {
            var gameImagesLocationSetting = ConfigurationManager.AppSettings[GameImagesLocation];
            var gameImagesLocation = new DirectoryInfo(gameImagesLocationSetting);

            var replacementImagesLocationSetting = ConfigurationManager.AppSettings[ReplacementImagesLocation];
            var replacementImagesLocation = new DirectoryInfo(replacementImagesLocationSetting);

            return CreateArtworkList(gameImagesLocation,replacementImagesLocation);
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

        private List<ArtworkModel> CreateArtworkList(DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            List<FileInfo> gameImageFiles = FindFiles(gameImagesLocation);
            List<FileInfo> replacementImageFiles = FindFiles(replacementImagesLocation);

            Debug.WriteLine($"Number of images found in {gameImagesLocation.FullName}: {gameImageFiles.Count}");

            var artworkList = new List<ArtworkModel>();

            foreach (var gameFile in gameImageFiles)
            {
                var replacementCard = FindSuitableReplacementCard(gameFile, replacementImageFiles);

                artworkList.Add(new ArtworkModel()
                {
                    GameImagePath = gameFile.FullName,
                    GameImageFileName = gameFile.Name,
                    ReplacementImagePath = replacementCard.ImagePath,
                    ReplacementImageMonsterName = replacementCard.CardName,
                    ReplacementImageFileName = replacementCard.ReplacementImageFileName
                });
            }
            return artworkList;
        }

        private ReplacementCard FindSuitableReplacementCard(FileInfo gameFile, List<FileInfo> replacementImageFiles)
        {
            Random r = new Random();
            int rInt = r.Next(0, replacementImageFiles.Count); 

            var randomCard = replacementImageFiles[rInt];
            return new ReplacementCard()
            {
                CardName = randomCard.Name,
                ImagePath = randomCard.FullName,
                ReplacementImageFileName = randomCard.Name
            };
        }

        private List<FileInfo> FindFiles(DirectoryInfo gameImagesLocation)
        {
            return Directory
                .EnumerateFiles(gameImagesLocation.FullName)
                .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png"))
                .Select(x => new FileInfo(x))
                .ToList();
        }
    }
}
