using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IBlueEyesLogic
    {
        void RunMatchAll();
        void RunSaveMatch();
        void RunLoadMatch(string path);
        void SavePathSetting(string filePath);
        void RunCustomArtPicked(Artwork artwork, int rowIndex, ArtworkSearch pickedArtwork);
    }
}