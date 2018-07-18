using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Presenter
{
    public class ArtworkEditorPresenter : IArtworkEditorPresenter
    {
        private readonly IArtworkEditor _artworkEditorUi;
        private readonly IBlueEyesLogic _blueEyesLogic;

        public ArtworkEditorPresenter(IArtworkEditor artworkEditorUi)
        {
            _artworkEditorUi = artworkEditorUi;
            _blueEyesLogic = new BlueEyesLogic(this);
        }

        public object ReplacementImageGetter(object row)
        {
            if (row == null) return null;

            var artworkRow = ((Artwork)row);
            var replacementImagePath = artworkRow.ReplacementImageFilePath;

            if (string.IsNullOrEmpty(replacementImagePath))
            {
                throw new ArgumentNullException($"Can not load ReplacementImageGetter for: {artworkRow}");
            }

            if (_artworkEditorUi.SmallImageListContains(replacementImagePath)) return replacementImagePath;

            Image image = Image.FromFile(replacementImagePath);
            UpdateImageLists(replacementImagePath, image);
            artworkRow.ReplacementImageWidth = image.Width;
            artworkRow.ReplacementImageHeight = image.Height;
            return replacementImagePath;
        }

        public object GameImageGetter(object row)
        {
            if (row == null) return null;

            var artworkRow = ((Artwork)row);
            var gameImagePath = artworkRow.GameImageFilePath;

            if (string.IsNullOrEmpty(gameImagePath))
            {
                throw new ArgumentNullException($"Can not load GameImageGetter for: {artworkRow}");
            }

            if (_artworkEditorUi.SmallImageListContains(gameImagePath)) return gameImagePath;

            Image image = Image.FromFile(gameImagePath);
            UpdateImageLists(gameImagePath, image);
            artworkRow.GameImageWidth = image.Width;
            artworkRow.GameImageHeight = image.Height;
            return gameImagePath;
        }

        private void UpdateImageLists(string imagePath, Image image)
        {
            _artworkEditorUi.SmallImageListAdd(imagePath, image);
        }

        public void MatchAll()
        {
            _blueEyesLogic.RunMatchAll();
        }

        public void Save()
        {
            try
            {
                _blueEyesLogic.RunSaveMatch();
            }
            catch (Exception e)
            {
                AppendConsoleException(e);
            }
        }

        private void AppendConsoleException(Exception exception)
        {
            var message = $"Exception: {exception.Message} \\r\\n InnerException: {exception.InnerException?.Message}";
            AppendConsoleText(message);
        }

        public void Load(string path)
        {
            try
            {
                _blueEyesLogic.RunLoadMatch(path);
            }
            catch (Exception e)
            {
                AppendConsoleException(e);
            }
        }

        public int GetConsoleLineNumber()
        {
            return _artworkEditorUi.GetConsoleLineNumber();
        }

        public void AppendConsoleText(string formattedMessage)
        {
            _artworkEditorUi.AppendConsoleText(formattedMessage);
        }

        public void RemoveOldestLine()
        {
            _artworkEditorUi.RemoveOldestLine();
        }

        public void SavePathSetting(string filePath)
        {
            try
            {
                _blueEyesLogic.SavePathSetting(filePath);
            }
            catch (Exception e)
            {
                AppendConsoleException(e);
            }
        }

        public void ShowMessageBox(string message)
        {
            _artworkEditorUi.ShowMessageBox(message);
        }

        public void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList)
        {
            _artworkEditorUi.AddObjectsToObjectListView(artworkList);
        }

        public void ClearObjectsFromObjectListView()
        {
            _artworkEditorUi.ClearObjectsFromObjectListView();
        }

        public void OpenCustomArtPicker(Artwork artwork, int rowIndex)
        {
            using (var artworkPicker = new ArtworkPicker(artwork))
            {
                switch (artworkPicker.ShowDialog())
                {
                    case DialogResult.OK:
                        var pickedArtwork = artworkPicker.ArtworkSearch;
                        _blueEyesLogic.RunCustomArtPicked(artwork, rowIndex, pickedArtwork);
                        _artworkEditorUi.RefreshObject(artwork);
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }
    }
}
