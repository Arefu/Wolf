using System;
using System.Collections.Generic;
using System.Drawing;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IArtworkEditor : IArtworkObjectListView, IConsoleLogUi, IDisposable
    {
        event Func<object, object> GameImageGetterEvent;
        event Func<object, object> ReplacementImageGetterEvent;
        event Action<string> MatchAllAction;
        event Action<IEnumerable<Artwork>> ConvertAllAction;
        event Action<Artwork, ArtworkSearch> CustomArtPickedAction;
        event Action<IEnumerable<Artwork>> SaveAction;
        event Action<string> LoadAction;
        event Action<string, Constants.Setting> SavePathSettingAction;
        event Action<bool> UsePendulumCheckedChanged;
        event Action<string> CardDbPathChanged;
        void ShowMessageBox(string message);
        void SetLoadPath(string path);
        void SetReplacementImagesPath(string path);
        void SetCardDbPath(string path);
    }
}
