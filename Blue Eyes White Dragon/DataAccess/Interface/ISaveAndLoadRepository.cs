using System.Collections.Generic;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface ISaveAndLoadRepository
    {
        string SaveArtworkMatchToFile(IEnumerable<Artwork> artworkList);
        IEnumerable<Artwork> LoadArtworkMatchFromFile(string path);
    }
}