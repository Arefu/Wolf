using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class ArtworkEditor : Form, IArtworkEditor
    {
        public event Func<object, object> GameImageGetterEvent;
        public event Func<object, object> ReplacementImageGetterEvent;
        public event Action<string, string> MatchAllAction;
        public event Action<Artwork, ArtworkSearch> CustomArtPickedAction;
        public event Action<IEnumerable<Artwork>> SaveAction;
        public event Action<string> LoadAction;
        public event Action<string, Constants.Setting> SavePathSettingAction;
        public event Action<bool> UsePendulumCheckedChanged;

        private readonly IArtworkPickerPresenterFactory _artworkPickerPresenterFactory;
        private const int StartingNumberOfRowIndex = 1;

        public ArtworkEditor(IArtworkPickerPresenterFactory artworkPickerPresenterFactory) 
        {
            _artworkPickerPresenterFactory = artworkPickerPresenterFactory ?? throw new ArgumentNullException(nameof(artworkPickerPresenterFactory));
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SetupColumns();
            SetupButtons();
        }

        private void SetupButtons()
        {
            BtnCustomArt.AspectGetter = x => Localization.BtnTextCustom;
            objlist_artwork_editor.ButtonClick += CustomArtClicked;
        }

        public void AddObjectsToObjectListView(IEnumerable<IUiModel> artworkList)
        {
            objlist_artwork_editor.AddObjects(artworkList.ToList());
        }

        private void SetupColumns()
        {
            GI.AspectGetter = x => ((Artwork) x)?.GameImageFile.FullName;
            GI.ImageGetter = x => GameImageGetterEvent;
            GIFileName.AspectGetter = x => ((Artwork) x)?.GameImageFileName;
            GICardName.AspectGetter = x => ((Artwork) x)?.GameImageMonsterName;
            GIWidth.AspectGetter = x => ((Artwork)x)?.GameImageWidth;
            GIHeight.AspectGetter = x => ((Artwork)x)?.GameImageHeight;

            RI.AspectGetter = x => ((Artwork)x)?.ReplacementImageFile.FullName;
            RI.ImageGetter = x => ReplacementImageGetterEvent;
            RICardName.AspectGetter = x => ((Artwork)x)?.ReplacementImageMonsterName;
            RIFileName.AspectGetter = x => ((Artwork)x)?.ReplacementImageFileName;
            RIWidth.AspectGetter = x => ((Artwork) x)?.ReplacementImageWidth;
            RIHeight.AspectGetter = x => ((Artwork) x)?.ReplacementImageHeight;

            Row.AspectGetter = x => objlist_artwork_editor.IndexOf(x) + StartingNumberOfRowIndex;

            HasAltImages.AspectGetter = x =>
            {
                var row = ((Artwork) x);
                if (row == null) return "";
                if (row.AlternateReplacementImages.Any())
                {
                    return Localization.BtnTextYes;
                }
                else
                {
                    return Localization.BtnTextBlank;
                }
            };
        }

        private void CustomArtClicked(object sender, CellClickEventArgs e)
        {
            var artwork = (Artwork) e.Model;
            OpenArtworkPicker(artwork);
        }

        private void FilterRows()
        {
            var filter = TextMatchFilter.Contains(objlist_artwork_editor, txt_search.Text);

            if (objlist_artwork_editor.DefaultRenderer == null)
            {
                objlist_artwork_editor.DefaultRenderer = new HighlightTextRenderer(filter);
            }

            objlist_artwork_editor.AdditionalFilter = filter;
        }

        private void Btn_run_Click(object sender, EventArgs e)
        {
            var gameImagesLocation = txt_browse_game_images.Text;
            var replacementImagesLocation = txt_browse_replacement_images.Text;
            MatchAllAction?.Invoke(gameImagesLocation, replacementImagesLocation);
        }

        private void Txt_search_TextChanged(object sender, EventArgs e)
        {
            FilterRows();
        }

        public bool SmallImageListContains(string gameImagePath)
        {
            return objlist_artwork_editor.SmallImageList.Images.ContainsKey(gameImagePath);
        }

        public void SmallImageListAdd(string imagePath, Image smallImage)
        {
            objlist_artwork_editor.SmallImageList.Images.Add(imagePath, smallImage);
        }

        public int GetConsoleLineNumber()
        {
            return richtextbox_console.Lines.Length;
        }

        public void AppendConsoleText(string message)
        {
            richtextbox_console.AppendText(message);
        }

        public void RemoveOldestLine()
        {
            var lines = richtextbox_console.Lines.ToList();
            lines.RemoveAt(0);
            richtextbox_console.Lines = lines.ToArray();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ClearObjectsFromObjectListView()
        {
            objlist_artwork_editor.ClearObjects();
        }

        public void RefreshObject(IUiModel artwork)
        {
            objlist_artwork_editor.RefreshObject(artwork);
        }

        public void SetLoadPath(string path)
        {
            txt_card_match_path.Text = path;
        }

        public void SetGameImagesPath(string path)
        {
            txt_browse_game_images.Text = path;
        }

        public void SetReplacementImagesPath(string path)
        {
            txt_browse_replacement_images.Text = path;
        }

        public void SetCardDbPath(string path)
        {
            txt_browse_carddb.Text = path;
        }

        private void OpenArtworkPicker(Artwork artwork)
        {
            using (var artworkPickerPresenter = _artworkPickerPresenterFactory.NewArtworkPickerPresenter())
            {
                artworkPickerPresenter.SetCurrentArtwork(artwork);
                switch (artworkPickerPresenter.ShowDialog())
                {
                    case DialogResult.OK:
                        var pickedArtwork = artworkPickerPresenter.ArtworkSearchResult;
                        CustomArtPickedAction?.Invoke(artwork, pickedArtwork);
                        RefreshObject(artwork);
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }

        private void Btn_save_match_Click(object sender, EventArgs e)
        {
            var objects = (ArrayList) objlist_artwork_editor.Objects;
            var artworks = objects.Cast<Artwork>().ToList();
            SaveAction?.Invoke(artworks);
        }

        private void Btn_load_match_Click(object sender, EventArgs e)
        {
            LoadAction?.Invoke(txt_card_match_path.Text);
        }

        private void Richtextbox_console_TextChanged(object sender, EventArgs e)
        {
            richtextbox_console.SelectionStart = richtextbox_console.TextLength;
            richtextbox_console.ScrollToCaret();
        }

        private void Btn_browse_load_match_path_Click(object sender, EventArgs e)
        {
            if (browse_json_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = browse_json_file.FileName;
                SavePathSettingAction?.Invoke(filePath, Constants.Setting.LastUsedLoadPath);
                txt_card_match_path.Text = filePath;
            }
        }

        private void Btn_browse_game_images_Click(object sender, EventArgs e)
        {
            if (browse_open_folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = browse_open_folder.SelectedPath;
                SavePathSettingAction?.Invoke(filePath, Constants.Setting.LastUsedGameImagePath);
                txt_browse_game_images.Text = filePath;
            }
        }

        private void Btn_browse_replacement_images_Click(object sender, EventArgs e)
        {
            if (browse_open_folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = browse_open_folder.SelectedPath;
                SavePathSettingAction?.Invoke(filePath, Constants.Setting.LastUsedReplacementImagePath);
                txt_browse_replacement_images.Text = filePath;
            }
        }

        private void Chckbx_use_pendulum_CheckedChanged(object sender, EventArgs e)
        {
            UsePendulumCheckedChanged?.Invoke(checkBox1.Checked);
        }

        private void Btn_browse_carddb_Click(object sender, EventArgs e)
        {
            if (browse_carddb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = browse_carddb.FileName;
                SavePathSettingAction?.Invoke(filePath, Constants.Setting.LasstUsedCardDbPath);
                txt_browse_carddb.Text = filePath;
            }
        }
    }
}

    
