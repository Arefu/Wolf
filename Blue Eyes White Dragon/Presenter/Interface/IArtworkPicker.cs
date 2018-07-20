using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter.Interface
{
    public interface IArtworkPicker : IDisposable
    {
        DialogResult ShowDialog();
        ArtworkSearch ArtworkSearchResult { get; }
        event Action<Artwork> LoadAlternateImages;
        event Action<string> SearchCards;
        event Action<ArtworkSearch> CardPicked;
        event Func<object, object> ImageGetter;
        bool SmallImageListContains(string imagePath);
        void SmallImageListAdd(string imagePath, Image image);
        void AddObjectsToObjectListView(IEnumerable<ArtworkSearch> artworkSearchList);
        void SetCurrentArtwork(Artwork artwork);
    }
}