namespace Blue_Eyes_White_Dragon.UI
{
    partial class ArtworkPicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.objlist_artwork_picker = new BrightIdeasSoftware.FastObjectListView();
            this.Row = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CardName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.CardImage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ImageRenderer = new BrightIdeasSoftware.ImageRenderer();
            this.BtnOk = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_search = new System.Windows.Forms.Button();
            this.lbl_how_to_search = new System.Windows.Forms.Label();
            this.txtbox_search = new System.Windows.Forms.TextBox();
            this.btn_cancel_pick = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objlist_artwork_picker)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objlist_artwork_picker
            // 
            this.objlist_artwork_picker.AllColumns.Add(this.Row);
            this.objlist_artwork_picker.AllColumns.Add(this.CardName);
            this.objlist_artwork_picker.AllColumns.Add(this.CardImage);
            this.objlist_artwork_picker.AllColumns.Add(this.BtnOk);
            this.objlist_artwork_picker.CellEditUseWholeCell = false;
            this.objlist_artwork_picker.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Row,
            this.CardName,
            this.CardImage,
            this.BtnOk});
            this.objlist_artwork_picker.Cursor = System.Windows.Forms.Cursors.Default;
            this.objlist_artwork_picker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objlist_artwork_picker.Location = new System.Drawing.Point(0, 109);
            this.objlist_artwork_picker.Name = "objlist_artwork_picker";
            this.objlist_artwork_picker.RowHeight = 256;
            this.objlist_artwork_picker.ShowGroups = false;
            this.objlist_artwork_picker.Size = new System.Drawing.Size(803, 745);
            this.objlist_artwork_picker.SmallImageList = this.imageList1;
            this.objlist_artwork_picker.TabIndex = 0;
            this.objlist_artwork_picker.UseCompatibleStateImageBehavior = false;
            this.objlist_artwork_picker.View = System.Windows.Forms.View.Details;
            this.objlist_artwork_picker.VirtualMode = true;
            // 
            // Row
            // 
            this.Row.Text = "Row";
            this.Row.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Row.Width = 35;
            // 
            // CardName
            // 
            this.CardName.Text = "Card Name";
            this.CardName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CardName.Width = 100;
            this.CardName.WordWrap = true;
            // 
            // CardImage
            // 
            this.CardImage.Renderer = this.ImageRenderer;
            this.CardImage.Searchable = false;
            this.CardImage.Text = "Image";
            this.CardImage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CardImage.UseFiltering = false;
            this.CardImage.Width = 256;
            // 
            // BtnOk
            // 
            this.BtnOk.ButtonSize = new System.Drawing.Size(50, 20);
            this.BtnOk.IsButton = true;
            this.BtnOk.Text = "Choose Image";
            this.BtnOk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BtnOk.Width = 100;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_cancel_pick);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.lbl_how_to_search);
            this.panel1.Controls.Add(this.txtbox_search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 109);
            this.panel1.TabIndex = 1;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(297, 40);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 2;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.Btn_search_Click);
            // 
            // lbl_how_to_search
            // 
            this.lbl_how_to_search.AutoSize = true;
            this.lbl_how_to_search.Location = new System.Drawing.Point(32, 26);
            this.lbl_how_to_search.Name = "lbl_how_to_search";
            this.lbl_how_to_search.Size = new System.Drawing.Size(130, 13);
            this.lbl_how_to_search.TabIndex = 1;
            this.lbl_how_to_search.Text = "Search the card database";
            // 
            // txtbox_search
            // 
            this.txtbox_search.Location = new System.Drawing.Point(35, 42);
            this.txtbox_search.Name = "txtbox_search";
            this.txtbox_search.Size = new System.Drawing.Size(256, 20);
            this.txtbox_search.TabIndex = 0;
            // 
            // btn_cancel_pick
            // 
            this.btn_cancel_pick.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel_pick.Location = new System.Drawing.Point(716, 42);
            this.btn_cancel_pick.Name = "btn_cancel_pick";
            this.btn_cancel_pick.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel_pick.TabIndex = 3;
            this.btn_cancel_pick.Text = "Cancel";
            this.btn_cancel_pick.UseVisualStyleBackColor = true;
            this.btn_cancel_pick.Click += new System.EventHandler(this.Btn_cancel_pick_Click);
            // 
            // ArtworkPicker
            // 
            this.AcceptButton = this.btn_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel_pick;
            this.ClientSize = new System.Drawing.Size(803, 854);
            this.Controls.Add(this.objlist_artwork_picker);
            this.Controls.Add(this.panel1);
            this.Name = "ArtworkPicker";
            this.Text = "Artwork Picker";
            ((System.ComponentModel.ISupportInitialize)(this.objlist_artwork_picker)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView objlist_artwork_picker;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label lbl_how_to_search;
        private System.Windows.Forms.TextBox txtbox_search;
        private BrightIdeasSoftware.OLVColumn Row;
        private BrightIdeasSoftware.OLVColumn CardName;
        private BrightIdeasSoftware.OLVColumn CardImage;
        private BrightIdeasSoftware.OLVColumn BtnOk;
        private System.Windows.Forms.ImageList imageList1;
        private BrightIdeasSoftware.ImageRenderer ImageRenderer;
        private System.Windows.Forms.Button btn_cancel_pick;
    }
}