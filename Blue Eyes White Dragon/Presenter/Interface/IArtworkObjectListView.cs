using System;
using System.Collections.Generic;
using System.Drawing;
using Blue_Eyes_White_Dragon.UI.Interface;

namespace Blue_Eyes_White_Dragon.Presenter.Interface
{
    public interface IArtworkObjectListView
    {
        bool SmallImageListContains(string imagePath);
        void SmallImageListAdd(string imagePath, Image image);
        void ClearObjectsFromObjectListView();
        void AddObjectsToObjectListView(IEnumerable<IUiModel> artworkSearchList);
        void RefreshObject(IUiModel artwork);
    }
}
