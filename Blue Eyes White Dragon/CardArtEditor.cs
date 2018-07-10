using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon
{
    public partial class CardArtEditor : Form
    {
        public CardArtEditor()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var gameCardsLocationSetting = ConfigurationManager.AppSettings["GameCardsLocation"];
            var gameCardsLocation = new DirectoryInfo(gameCardsLocationSetting);

            var gameCardImages = LoadImagesFromDisk(gameCardsLocation);
            var artworkList = CreateArtworkList(gameCardsLocation);

            var imageList = new ImageList();
            imageList.Images.AddRange(artworkList.Select(x => x.GameImage).ToArray());
            objectListView1.LargeImageList = imageList;
            
            GameImage.ImageGetter = new ImageGetterDelegate(GameImageGetter);

            objectListView1.AddObjects(artworkList);

            objectListView1.EnsureModelVisible(artworkList);
        }

        public object GameImageGetter(object rowObject)
        {
            ArtworkModel s = (ArtworkModel)rowObject;
            return s.Index;
        }

        private ImageList LoadImagesFromDisk(DirectoryInfo dir)
        {
            var files = dir.GetFiles("*.*");
            var imageList = new ImageList();

            for (int i = 0; i < 100; i++)
            {
                imageList.Images.Add(Image.FromFile(files[i].FullName));
            }

            return imageList;
        }

        private List<ArtworkModel> CreateArtworkList(DirectoryInfo dir)
        {
            var files = dir.GetFiles("*.*");
            var artworkList = new List<ArtworkModel>();

            for (int i = 0; i < 100; i++)
            {
                artworkList.Add(new ArtworkModel()
                {
                    Index = i,
                    GameImage = Image.FromFile(files[i].FullName),
                    GameImageFormat = files[i].Extension
                });
            }

            return artworkList;
        }

    }
}
