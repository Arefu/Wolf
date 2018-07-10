namespace Blue_Eyes_White_Dragon
{
    partial class CardArtEditor
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
            this.imageRenderer1 = new BrightIdeasSoftware.ImageRenderer();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.GameImage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.GameImage);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GameImage});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.RowHeight = 256;
            this.objectListView1.Size = new System.Drawing.Size(1067, 603);
            this.objectListView1.TabIndex = 2;
            this.objectListView1.TileSize = new System.Drawing.Size(304, 30);
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // GameImage
            // 
            this.GameImage.AspectName = "GameImage";
            this.GameImage.Groupable = false;
            this.GameImage.IsEditable = false;
            this.GameImage.Renderer = this.imageRenderer1;
            this.GameImage.Searchable = false;
            this.GameImage.Sortable = false;
            this.GameImage.Text = "GameImage";
            this.GameImage.Width = 304;
            // 
            // CardArtEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 603);
            this.Controls.Add(this.objectListView1);
            this.Name = "CardArtEditor";
            this.Text = "Card_Art_Editor";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ImageRenderer imageRenderer1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn GameImage;
    }
}