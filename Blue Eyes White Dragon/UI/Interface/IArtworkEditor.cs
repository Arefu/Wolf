using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.UI.Interface
{
    public interface IArtworkEditor
    {
        void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList);
        bool SmallImageListContains(string gameImagePath);
        void SmallImageListAdd(string imagePath, Image smallImage);
        int GetConsoleLineNumber();
        void AppendConsoleText(string message);
        void RemoveOldestLine();
        void ShowMessageBox(string empty);
        void ClearObjectsFromObjectListView();
    }
}
