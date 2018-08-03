using System;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter.Interface
{
    public interface IArtworkPicker : IArtworkObjectListView, IDisposable
    {
        DialogResult ShowDialog();
        ArtworkSearch ArtworkSearchResult { get; }
        event Action<Artwork> LoadAlternateImages;
        event Action<string> SearchCards;
        event Action<ArtworkSearch> CardPicked;
        event Func<object, object> ImageGetter;
        void SetCurrentArtwork(IUiModel artwork);
    }
}