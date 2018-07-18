using System.Collections.Generic;
using System.Drawing;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter
{
    public interface IArtworkPicker
    {
        bool SmallImageListContains(string imagePath);
        void SmallImageListAdd(string imagePath, Image image);
        void AddObjectsToObjectListView(IEnumerable<ArtworkSearch> artworkSearch);
    }
}