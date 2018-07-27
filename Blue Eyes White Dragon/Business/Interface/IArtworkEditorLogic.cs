using System.Collections.Generic;
using System.IO;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IArtworkEditorLogic
    {
        IEnumerable<Artwork> RunMatchAll(DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation, bool useIncludedPendulum);
        void RunSaveMatch(IEnumerable<Artwork> artworks);
        void SavePathSetting(string filePath, Constants.Setting setting);
        void RunCustomArtPicked(Artwork artwork, ArtworkSearch pickedArtwork);
        IEnumerable<Artwork> LoadArtworkMatch(string path);
        void CalculateHeightAndWidth(List<Artwork> artworkList);
        IEnumerable<Artwork> SortArtwork(IEnumerable<Artwork> artworkList);
        string GetPathSetting(Constants.Setting setting);
        void RunConvertAll(IEnumerable<Artwork> artworks);
        void SetDbLocation(string path);
        string LoadDbPath();
    }
}