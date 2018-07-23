using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class ArtworkPicker : Form, IArtworkPicker
    {
        public ArtworkSearch ArtworkSearchResult { get; private set; }
        private Artwork _currentArtwork;
        public event Action<Artwork> LoadAlternateImages;
        public event Action<string> SearchCards;
        public event Action<ArtworkSearch> CardPicked;
        public event Func<object, object> ImageGetter;

        public ArtworkPicker()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SetupColumns();
            SetupButtons();
        }

        public void SetCurrentArtwork(IUiModel artwork)
        {
            _currentArtwork = (Artwork)artwork;
            LoadData();
        }

        private void LoadData()
        {
            LoadAlternateImages?.Invoke(_currentArtwork);
        }

        private void SetupColumns()
        {
            Row.AspectGetter = x => objlist_artwork_picker.IndexOf(x) + 1;
            CardName.AspectGetter = x => ((ArtworkSearch) x)?.CardName;
            CardImage.AspectGetter = x => ((ArtworkSearch) x)?.ImageFilePath;
            CardImage.ImageGetter = x => ImageGetter;
        }

        private void SetupButtons()
        {
            BtnOk.AspectGetter = x => Localization.BtnTextOk;
            objlist_artwork_picker.ButtonClick += OkClicked;
        }

        private void OkClicked(object sender, CellClickEventArgs e)
        {
            CardPicked?.Invoke((ArtworkSearch)e.Model);
            DialogResult = DialogResult.OK;
        }

        public bool SmallImageListContains(string imagePath)
        {
            return objlist_artwork_picker.SmallImageList.Images.ContainsKey(imagePath);
        }

        public void SmallImageListAdd(string imagePath, Image image)
        {
            objlist_artwork_picker.SmallImageList.Images.Add(imagePath, image);
        }

        public void AddObjectsToObjectListView(IEnumerable<IUiModel> artworkSearchList)
        {
            objlist_artwork_picker.AddObjects(artworkSearchList.ToList());
        }

        public void RefreshObject(IUiModel artwork)
        {
            objlist_artwork_picker.RefreshObject(artwork);
        }

        private void Btn_search_Click(object sender, EventArgs e)
        {
            SearchCards?.Invoke(txtbox_search.Text);
        }

        private void Btn_cancel_pick_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void ClearObjectsFromObjectListView()
        {
            objlist_artwork_picker.ClearObjects();
        }
    }
}
