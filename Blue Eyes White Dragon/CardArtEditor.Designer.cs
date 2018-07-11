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
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.GameImage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.AllColumns.Add(this.GameImage);
            this.fastObjectListView1.CellEditUseWholeCell = false;
            this.fastObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GameImage});
            this.fastObjectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastObjectListView1.Location = new System.Drawing.Point(0, 0);
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.RowHeight = 256;
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(1067, 603);
            this.fastObjectListView1.TabIndex = 0;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            // 
            // GameImage
            // 
            this.GameImage.Groupable = false;
            this.GameImage.IsEditable = false;
            this.GameImage.MinimumWidth = 256;
            this.GameImage.Renderer = this.imageRenderer1;
            this.GameImage.Searchable = false;
            this.GameImage.Text = "GameImage";
            this.GameImage.Width = 256;
            // 
            // CardArtEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 603);
            this.Controls.Add(this.fastObjectListView1);
            this.Name = "CardArtEditor";
            this.Text = "Card_Art_Editor";
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ImageRenderer imageRenderer1;
        private BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private BrightIdeasSoftware.OLVColumn GameImage;
    }
}