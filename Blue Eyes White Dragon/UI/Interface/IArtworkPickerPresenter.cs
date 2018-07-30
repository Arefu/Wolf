using System;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IArtworkPickerPresenter : IDisposable
    {
        ArtworkSearch ArtworkSearchResult { get; }
        object ImageGetter(object row);
        void LoadAlternateArtwork(Artwork artwork);
        void SearchArtwork(string text);
        DialogResult ShowDialog();
        void SetCurrentArtwork(Artwork artwork);
    }
}