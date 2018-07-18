﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Presenter;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.UI
{
    public partial class ArtworkEditor : Form, IArtworkEditor
    {
        ///The designer threw up when I tried to assign the ImageLists declaratively
        ///so we are doing it yolo style.
        private readonly ImageList _smallImageList;
        private readonly IArtworkEditorPresenter _artworkEditorPresenter;

        public ArtworkEditor()
        {
            InitializeComponent();
            _smallImageList = new ImageList();
            _artworkEditorPresenter = new ArtworkEditorPresenter(this);
            Init();
        }

        private void Init()
        {
            SetupImageList();
            SetupColumns();
            LoadSettings();
        }

        private void LoadSettings()
        {
            txt_card_match_path.Text = Properties.Settings.Default.LastUsedLoadPath;
        }

        public void AddObjectsToObjectListView(IEnumerable<Artwork> artworkList)
        {
            objlist_artwork_editor.AddObjects(artworkList.ToList());
        }

        private void SetupImageList()
        {
            _smallImageList.ColorDepth = ColorDepth.Depth24Bit;
            _smallImageList.ImageSize = new Size(256, 256);
            objlist_artwork_editor.SmallImageList = _smallImageList;
        }

        private void SetupColumns()
        {
            //Apparently something other than the image must be shown in the coloumn for the image to be visible
            GI.AspectGetter = x => ((Artwork) x)?.GameImageFilePath;
            //The image that actually will be shown instead of the above path string
            GI.ImageGetter = _artworkEditorPresenter.GameImageGetter;
            GIFileName.AspectGetter = x => ((Artwork) x)?.GameImageFileName;
            GICardName.AspectGetter = x => ((Artwork) x)?.GameImageMonsterName;

            GIWidth.AspectGetter = x => ((Artwork)x)?.GameImageWidth;
            //GIHeight.AspectGetter = x => ((Artwork)x).GameImageHeight;

            RI.AspectGetter = x => ((Artwork)x)?.ReplacementImageFilePath;
            RI.ImageGetter = _artworkEditorPresenter.ReplacementImageGetter;
            RICardName.AspectGetter = x => ((Artwork)x)?.ReplacementImageMonsterName;
            RIFileName.AspectGetter = x => ((Artwork)x)?.ReplacementImageFileName;

            RIWidth.AspectGetter = x => ((Artwork) x)?.ReplacementImageWidth;
            RIHeight.AspectGetter = x => ((Artwork) x)?.ReplacementImageHeight;

            //Makes the row count from 1 instead of 0
            Row.AspectGetter = x => objlist_artwork_editor.IndexOf(x)+1;
            
        }

        private void FilterRows()
        {
            var filter = TextMatchFilter.Contains(objlist_artwork_editor, txt_search.Text);

            if (objlist_artwork_editor.DefaultRenderer == null)
            {
                objlist_artwork_editor.DefaultRenderer = new HighlightTextRenderer(filter);
            }

            var a = objlist_artwork_editor.FilteredObjects;
            objlist_artwork_editor.AdditionalFilter = filter;
            var b = objlist_artwork_editor.FilteredObjects;
        }

        private void Btn_run_Click(object sender, EventArgs e)
        {
            _artworkEditorPresenter.MatchAll();
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

        private void Btn_save_match_Click(object sender, EventArgs e)
        {
            _artworkEditorPresenter.Save();
        }

        private void Btn_load_match_Click(object sender, EventArgs e)
        {
            _artworkEditorPresenter.Load(txt_card_match_path.Text);
        }

        private void Richtextbox_console_TextChanged(object sender, EventArgs e)
        {
            richtextbox_console.SelectionStart = richtextbox_console.TextLength;
            richtextbox_console.ScrollToCaret();
        }

        private void Btn_browse_match_file_path_Click(object sender, EventArgs e)
        {
            if (open_file_browse_match_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = open_file_browse_match_file.FileName;
                _artworkEditorPresenter.SavePathSetting(filePath);
                txt_card_match_path.Text = filePath;
            }
        }
    }
}