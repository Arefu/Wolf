using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Presenter
{
    public class ArtworkEditorPresenter : IArtworkEditorPresenter
    {
        private readonly IArtworkEditorLogic _artworkEditorLogic;
        private readonly ILogger _logger;
        public IArtworkEditor View { get; }
        private bool UseIncludedPendulum = true;

        public ArtworkEditorPresenter(IArtworkEditor view, IArtworkEditorLogic artworkEditorLogic, ILogger logger)
        {
            _artworkEditorLogic = artworkEditorLogic ?? throw new ArgumentNullException(nameof(artworkEditorLogic));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            View = view ?? throw new ArgumentNullException(nameof(view));

            LoadEvents();
        }

        private void LoadEvents()
        {
            View.GameImageGetterEvent += GameImageGetter;
            View.ReplacementImageGetterEvent += ReplacementImageGetter;
            View.CustomArtPickedAction += CustomArtPicked;
            View.LoadAction += Load;
            View.SaveAction += Save;
            View.MatchAllAction += MatchAll;
            View.SavePathSettingAction += SavePathSetting;
            View.UsePendulumCheckedChanged += SetPendulumChecked;

            _logger.AppendTextToConsole += AppendConsoleText;
            _logger.AppendExceptionToConsole += AppendConsoleException;
        }

        public object ReplacementImageGetter(object row)
        {
            if (row == null) return null;

            var artworkRow = ((Artwork)row);
            var replacementImagePath = artworkRow.ReplacementImageFilePath;

            if (View.SmallImageListContains(replacementImagePath)) return replacementImagePath;

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

            if (View.SmallImageListContains(gameImagePath)) return gameImagePath;

            Image image = Image.FromFile(gameImagePath);
            UpdateImageLists(gameImagePath, image);
            artworkRow.GameImageWidth = image.Width;
            artworkRow.GameImageHeight = image.Height;
            return gameImagePath;
        }

        private void UpdateImageLists(string imagePath, Image image)
        {
            View.SmallImageListAdd(imagePath, image);
        }

        public void MatchAll()
        {
            View.ClearObjectsFromObjectListView();
            var artworkList = _artworkEditorLogic.RunMatchAll(UseIncludedPendulum);
            View.AddObjectsToObjectListView(artworkList);
        }

        public void Save(IEnumerable<Artwork> artworks)
        {
            try
            {
                _artworkEditorLogic.RunSaveMatch(artworks);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        private void AppendConsoleException(string message)
        {
            ClipTextBox();
            var formattedMessage = Localization.ExceptionFormattedForConsole(message);
            AppendConsoleText(formattedMessage);
        }

        private void ClipTextBox()
        {
            var textLength = GetConsoleLineNumber();

            if (textLength > Constants.ConsoleLimit)
            {
                View.RemoveOldestLine();
            }
        }

        public void Load(string path)
        {
            _logger.LogInformation(Localization.InformationLoading);
            if (string.IsNullOrEmpty(path))
            {
                View.ShowMessageBox(Localization.ErrorEnterPath);
                return;
            }

            var jsonFile = new FileInfo(path);
            if (!jsonFile.Exists)
            {
                View.ShowMessageBox(Localization.ErrorFileNotExist);
                return;
            }

            var artworks = _artworkEditorLogic.LoadArtworkMatch(path).ToList();

            if (artworks.Count == 0)
            {
                View.ShowMessageBox(Localization.ErrorFileEmptyOrInvalid);
                return;
            }

            View.ClearObjectsFromObjectListView();
            _artworkEditorLogic.CalculateHeightAndWidth(artworks);
            var sortedArtwork = _artworkEditorLogic.SortArtwork(artworks);

            View.AddObjectsToObjectListView(sortedArtwork);
            _logger.LogInformation(Localization.InformationLoadComplete(path));
        }

        private int GetConsoleLineNumber()
        {
            return View.GetConsoleLineNumber();
        }

        private void AppendConsoleText(string formattedMessage)
        {
            View.AppendConsoleText(formattedMessage);
        }

        public void SavePathSetting(string filePath)
        {
            try
            {
                _artworkEditorLogic.SavePathSetting(filePath);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        private void ShowMessageBox(string message)
        {
            View.ShowMessageBox(message);
        }

        private void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList)
        {
            View.AddObjectsToObjectListView(artworkList);
        }

        private void ClearObjectsFromObjectListView()
        {
            View.ClearObjectsFromObjectListView();
        }

        public void CustomArtPicked(Artwork artwork, ArtworkSearch pickedArtwork)
        {
            _artworkEditorLogic.RunCustomArtPicked(artwork, pickedArtwork);
        }

        private void SetPendulumChecked(bool isChecked)
        {
            UseIncludedPendulum = isChecked;
        }
    }
}
