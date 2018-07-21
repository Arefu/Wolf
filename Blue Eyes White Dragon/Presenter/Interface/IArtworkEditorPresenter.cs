using System.Collections.Generic;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter.Interface
{
    public interface IArtworkEditorPresenter
    {
        IArtworkEditor View { get; }
        object ReplacementImageGetter(object row);
        object GameImageGetter(object row);
        void MatchAll(string gameImagesLocation, string replacementImagesLocation);
        void Save(IEnumerable<Artwork> artworks);
        void Load(string path);
        void SavePathSetting(string filePath);
        void CustomArtPicked(Artwork artwork, ArtworkSearch pickedArtwork);
    }
}