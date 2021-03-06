﻿using System.Collections.Generic;
using System.IO;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IArtworkManager
    {
        List<Artwork> CreateArtworkModels(List<Card> gameCards, DirectoryInfo replacementImagesLocation);
        List<Artwork> UpdateArtworkModelsWithReplacement(IEnumerable<Artwork> artworkList, bool useIncludedPendulum);
        FileInfo SearchForImage(int cardId, DirectoryInfo directory);
        void ConvertAll(IEnumerable<Artwork> artworks);
        void FixMissingArtwork(List<Artwork> artworks);
    }
}