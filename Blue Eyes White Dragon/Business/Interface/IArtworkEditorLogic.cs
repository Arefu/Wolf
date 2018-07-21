using System.Collections.Generic;
using System.IO;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IArtworkEditorLogic
    {
        IEnumerable<Artwork> RunMatchAll(DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation, bool useIncludedPendulum);
        void RunSaveMatch(IEnumerable<Artwork> artworks);
        void SavePathSetting(string filePath);
        void RunCustomArtPicked(Artwork artwork, ArtworkSearch pickedArtwork);
        IEnumerable<Artwork> LoadArtworkMatch(string path);
        void CalculateHeightAndWidth(List<Artwork> artworkList);
        IEnumerable<Artwork> SortArtwork(IEnumerable<Artwork> artworkList);
    }
}