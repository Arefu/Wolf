using System.Collections.Generic;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter.Interface
{
    public interface IArtworkEditorPresenter
    {
        object ReplacementImageGetter(object row);
        object GameImageGetter(object row);
        void MatchAll();
        void Save();
        void Load(string path);
        int GetConsoleLineNumber();
        void AppendConsoleText(string formattedMessage);
        void RemoveOldestLine();
        void SavePathSetting(string filePath);
        void ShowMessageBox(string message);
        void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList);
        void ClearObjectsFromObjectListView();
        void OpenCustomArtPicker(Artwork artwork, int rowIndex);
    }
}