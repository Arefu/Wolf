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
            this.BtnOk = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 109);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(297, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter the name of a card";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(35, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(256, 20);
            this.textBox1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ArtworkPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private BrightIdeasSoftware.OLVColumn Row;
        private BrightIdeasSoftware.OLVColumn CardName;
        private BrightIdeasSoftware.OLVColumn CardImage;
        private BrightIdeasSoftware.OLVColumn BtnOk;
        private System.Windows.Forms.ImageList imageList1;
    }
}