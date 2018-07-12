namespace Blue_Eyes_White_Dragon.UI
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
            this.components = new System.ComponentModel.Container();
            this.imageRenderer1 = new BrightIdeasSoftware.ImageRenderer();
            this.imageRenderer2 = new BrightIdeasSoftware.ImageRenderer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.GIHeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GIWidth = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GIFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GICardName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GI = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RI = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RICardName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RIFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RIHeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.RIWidth = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.AllColumns.Add(this.GIHeight);
            this.fastObjectListView1.AllColumns.Add(this.GIWidth);
            this.fastObjectListView1.AllColumns.Add(this.GIFileName);
            this.fastObjectListView1.AllColumns.Add(this.GICardName);
            this.fastObjectListView1.AllColumns.Add(this.GI);
            this.fastObjectListView1.AllColumns.Add(this.RI);
            this.fastObjectListView1.AllColumns.Add(this.RICardName);
            this.fastObjectListView1.AllColumns.Add(this.RIFileName);
            this.fastObjectListView1.AllColumns.Add(this.RIHeight);
            this.fastObjectListView1.AllColumns.Add(this.RIWidth);
            this.fastObjectListView1.CellEditUseWholeCell = false;
            this.fastObjectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GIHeight,
            this.GIWidth,
            this.GIFileName,
            this.GICardName,
            this.GI,
            this.RI,
            this.RICardName,
            this.RIFileName,
            this.RIHeight,
            this.RIWidth});
            this.fastObjectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastObjectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastObjectListView1.Location = new System.Drawing.Point(0, 0);
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.RowHeight = 256;
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(1318, 1039);
            this.fastObjectListView1.TabIndex = 0;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.UseFiltering = true;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            // 
            // GIHeight
            // 
            this.GIHeight.Text = "Height";
            // 
            // GIWidth
            // 
            this.GIWidth.Text = "Width";
            // 
            // GIFileName
            // 
            this.GIFileName.Text = "File Name";
            this.GIFileName.Width = 100;
            // 
            // GICardName
            // 
            this.GICardName.Text = "Card Name";
            this.GICardName.Width = 100;
            this.GICardName.WordWrap = true;
            // 
            // GI
            // 
            this.GI.Groupable = false;
            this.GI.IsEditable = false;
            this.GI.MinimumWidth = 256;
            this.GI.Renderer = this.imageRenderer1;
            this.GI.Searchable = false;
            this.GI.Text = "Game Image";
            this.GI.Width = 256;
            // 
            // RI
            // 
            this.RI.Groupable = false;
            this.RI.IsEditable = false;
            this.RI.MinimumWidth = 256;
            this.RI.Renderer = this.imageRenderer2;
            this.RI.Searchable = false;
            this.RI.Text = "Replacement Image";
            this.RI.Width = 256;
            // 
            // RICardName
            // 
            this.RICardName.Text = "Card Name";
            this.RICardName.Width = 100;
            this.RICardName.WordWrap = true;
            // 
            // RIFileName
            // 
            this.RIFileName.Text = "File Name";
            this.RIFileName.Width = 100;
            // 
            // RIHeight
            // 
            this.RIHeight.Text = "Height";
            // 
            // RIWidth
            // 
            this.RIWidth.Text = "Width";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_run);
            this.groupBox1.Controls.Add(this.txt_search);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 1039);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // btn_run
            // 
            this.btn_run.Location = new System.Drawing.Point(23, 122);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(115, 40);
            this.btn_run.TabIndex = 1;
            this.btn_run.Text = "Auto Match All";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(9, 19);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(150, 20);
            this.txt_search.TabIndex = 0;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(171, 1039);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fastObjectListView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(171, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1318, 1039);
            this.panel2.TabIndex = 3;
            // 
            // CardArtEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 1039);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CardArtEditor";
            this.Text = "Card_Art_Editor";
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ImageRenderer imageRenderer1;
        private System.Windows.Forms.ImageList imageList1;
        private BrightIdeasSoftware.ImageRenderer imageRenderer2;
        private BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private BrightIdeasSoftware.OLVColumn GIHeight;
        private BrightIdeasSoftware.OLVColumn GIWidth;
        private BrightIdeasSoftware.OLVColumn GIFileName;
        private BrightIdeasSoftware.OLVColumn GICardName;
        private BrightIdeasSoftware.OLVColumn GI;
        private BrightIdeasSoftware.OLVColumn RI;
        private BrightIdeasSoftware.OLVColumn RICardName;
        private BrightIdeasSoftware.OLVColumn RIFileName;
        private BrightIdeasSoftware.OLVColumn RIHeight;
        private BrightIdeasSoftware.OLVColumn RIWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}