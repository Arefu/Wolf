using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter
{
    public class ArtworkPickerPresenter : IArtworkPickerPresenter
    {
        private readonly IArtworkPicker _artworkPickerUi;

        public ArtworkPickerPresenter(IArtworkPicker artworkPickerUi)
        {
            _artworkPickerUi = artworkPickerUi;
        }

        public object ImageGetter(object row)
        {
            if (row == null) return null;

            var artworkSearchRow = ((ArtworkSearch)row);
            var imagePath = artworkSearchRow.ImageFilePath;

            if (_artworkPickerUi.SmallImageListContains(imagePath)) return imagePath;

            Image image = Image.FromFile(imagePath);
            UpdateImageLists(imagePath, image);
            return imagePath;
        }

        public void LoadAlternateArtwork(Artwork artwork)
        {
            if (!artwork.AlternateReplacementImages.Any()) return;

            var artworkSearch = artwork.AlternateReplacementImages.Select(x => new ArtworkSearch()
            {
                CardName = artwork.GameImageMonsterName,
                ImageFile = x
            });
            _artworkPickerUi.AddObjectsToObjectListView(artworkSearch);
        }

        private void UpdateImageLists(string imagePath, Image image)
        {
            _artworkPickerUi.SmallImageListAdd(imagePath, image);
        }

    }
}
