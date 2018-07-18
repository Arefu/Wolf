using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    internal interface IArtworkPickerPresenter
    {
        object ImageGetter(object rowobject);
        void LoadAlternateArtwork(Artwork artwork);
    }
}