using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter
{
    public class ArtworkPickerPresenter : IArtworkPickerPresenter
    {
        public ArtworkSearch ArtworkSearchResult { get; private set; }
        private IArtworkPicker View { get; }
        private readonly IArtworkPickerLogic _artworkPickerLogic;

        public ArtworkPickerPresenter(IArtworkPicker view, IArtworkPickerLogic artworkPickerLogic)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            _artworkPickerLogic = artworkPickerLogic ?? throw new ArgumentNullException(nameof(artworkPickerLogic));

            LoadEvents();
        }

        private void LoadEvents()
        {
            View.LoadAlternateImages += LoadAlternateArtwork;
            View.SearchCards += SearchArtwork;
            View.ImageGetter += ImageGetter;
            View.CardPicked += CardPicked;
        }

        private void CardPicked(ArtworkSearch artworkSearch)
        {
            ArtworkSearchResult = artworkSearch;
        }

        public object ImageGetter(object row)
        {
            if (row == null) return null;

            var artworkSearchRow = ((ArtworkSearch)row);
            var imagePath = artworkSearchRow.ImageFilePath;

            if (View.SmallImageListContains(imagePath)) return imagePath;

            var image = Image.FromFile(imagePath);
            View.SmallImageListAdd(imagePath, image);
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
            View.AddObjectsToObjectListView(artworkSearch);
        }

        public void SearchArtwork(string text)
        {
            var searchResults = _artworkPickerLogic.SearchArtwork(text);
            View.ClearObjectsFromObjectListView();
            View.AddObjectsToObjectListView(searchResults);
        }

        public DialogResult ShowDialog()
        {
            return View.ShowDialog();
        }

        public void Dispose()
        {
            View?.Dispose();
        }

        public void SetCurrentArtwork(Artwork artwork)
        {
            View.SetCurrentArtwork(artwork);
        }
    }
}
