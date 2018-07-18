using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Presenter;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class ArtworkPicker : Form, IArtworkPicker
    {
        private readonly IArtworkPickerPresenter _artworkPickerPresenter;
        public ArtworkSearch ArtworkSearch;
        private ImageList _smallImageList;

        public ArtworkPicker(Artwork artwork)
        {
            InitializeComponent();
            _artworkPickerPresenter = new ArtworkPickerPresenter(this);
            Init(artwork);
        }

        private void Init(Artwork artwork)
        {
            SetupColumns();
            SetupButtons();
            LoadData(artwork);
        }

        private void LoadData(Artwork artwork)
        {
            _artworkPickerPresenter.LoadAlternateArtwork(artwork);
        }

        private void SetupColumns()
        {
            Row.AspectGetter = x => objlist_artwork_picker.IndexOf(x) + 1;
            CardName.AspectGetter = x => ((ArtworkSearch) x)?.CardName;
            CardImage.AspectGetter = x => ((ArtworkSearch) x)?.ImageFilePath;
            CardImage.ImageGetter = _artworkPickerPresenter.ImageGetter;
        }

        private void SetupButtons()
        {
            BtnOk.AspectGetter = x => Constants.BtnTextOk;
            objlist_artwork_picker.ButtonClick += OkClicked;
        }

        private void OkClicked(object sender, CellClickEventArgs e)
        {
            ArtworkSearch = (ArtworkSearch)e.Model;
            this.DialogResult = DialogResult.OK;
        }

        public bool SmallImageListContains(string imagePath)
        {
            return objlist_artwork_picker.SmallImageList.Images.ContainsKey(imagePath);
        }

        public void SmallImageListAdd(string imagePath, Image image)
        {
            objlist_artwork_picker.SmallImageList.Images.Add(imagePath, image);
        }

        public void AddObjectsToObjectListView(IEnumerable<ArtworkSearch> artworkSearchList)
        {
            objlist_artwork_picker.AddObjects(artworkSearchList.ToList());
        }
    }
}
