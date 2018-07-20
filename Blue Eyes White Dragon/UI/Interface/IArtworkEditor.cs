using System;
using System.Collections.Generic;
using System.Drawing;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IArtworkEditor : IDisposable
    {
        event Func<object, object> GameImageGetterEvent;
        event Func<object, object> ReplacementImageGetterEvent;
        event Action MatchAllAction;
        event Action<Artwork, ArtworkSearch> CustomArtPickedAction;
        event Action<IEnumerable<Artwork>> SaveAction;
        event Action<string> LoadAction;
        event Action<string> SavePathSettingAction;
        void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList);
        bool SmallImageListContains(string gameImagePath);
        void SmallImageListAdd(string imagePath, Image smallImage);
        int GetConsoleLineNumber();
        void AppendConsoleText(string message);
        void RemoveOldestLine();
        void ShowMessageBox(string message);
        void ClearObjectsFromObjectListView();
        void RefreshObject(Artwork artwork);
    }
}
