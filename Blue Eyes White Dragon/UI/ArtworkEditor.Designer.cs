namespace Blue_Eyes_White_Dragon.UI
{
    partial class ArtworkEditor
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
            this.objlist_artwork_editor = new BrightIdeasSoftware.FastObjectListView();
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
            this.pnl_object_list = new System.Windows.Forms.Panel();
            this.pnl_sidebar = new System.Windows.Forms.Panel();
            this.grpbox_console = new System.Windows.Forms.GroupBox();
            this.richtextbox_console = new System.Windows.Forms.RichTextBox();
            this.grpbox_left_top = new System.Windows.Forms.GroupBox();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.grpbox_bottom_left = new System.Windows.Forms.GroupBox();
            this.btn_save_match = new System.Windows.Forms.Button();
            this.grpbox_load = new System.Windows.Forms.GroupBox();
            this.btn_browse_match_file_path = new System.Windows.Forms.Button();
            this.btn_load_match = new System.Windows.Forms.Button();
            this.lbl_card_match_path = new System.Windows.Forms.Label();
            this.txt_card_match_path = new System.Windows.Forms.TextBox();
            this.btn_match_run = new System.Windows.Forms.Button();
            this.open_file_browse_match_file = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.objlist_artwork_editor)).BeginInit();
            this.pnl_object_list.SuspendLayout();
            this.pnl_sidebar.SuspendLayout();
            this.grpbox_console.SuspendLayout();
            this.grpbox_left_top.SuspendLayout();
            this.grpbox_bottom_left.SuspendLayout();
            this.grpbox_load.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // objlist_artwork_editor
            // 
            this.objlist_artwork_editor.AllColumns.Add(this.GIHeight);
            this.objlist_artwork_editor.AllColumns.Add(this.GIWidth);
            this.objlist_artwork_editor.AllColumns.Add(this.GIFileName);
            this.objlist_artwork_editor.AllColumns.Add(this.GICardName);
            this.objlist_artwork_editor.AllColumns.Add(this.GI);
            this.objlist_artwork_editor.AllColumns.Add(this.RI);
            this.objlist_artwork_editor.AllColumns.Add(this.RICardName);
            this.objlist_artwork_editor.AllColumns.Add(this.RIFileName);
            this.objlist_artwork_editor.AllColumns.Add(this.RIHeight);
            this.objlist_artwork_editor.AllColumns.Add(this.RIWidth);
            this.objlist_artwork_editor.CellEditUseWholeCell = false;
            this.objlist_artwork_editor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
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
            this.objlist_artwork_editor.Cursor = System.Windows.Forms.Cursors.Default;
            this.objlist_artwork_editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objlist_artwork_editor.Location = new System.Drawing.Point(0, 0);
            this.objlist_artwork_editor.Name = "objlist_artwork_editor";
            this.objlist_artwork_editor.RowHeight = 256;
            this.objlist_artwork_editor.ShowGroups = false;
            this.objlist_artwork_editor.Size = new System.Drawing.Size(1310, 1039);
            this.objlist_artwork_editor.TabIndex = 0;
            this.objlist_artwork_editor.UseCompatibleStateImageBehavior = false;
            this.objlist_artwork_editor.UseFiltering = true;
            this.objlist_artwork_editor.View = System.Windows.Forms.View.Details;
            this.objlist_artwork_editor.VirtualMode = true;
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
            // pnl_object_list
            // 
            this.pnl_object_list.Controls.Add(this.objlist_artwork_editor);
            this.pnl_object_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_object_list.Location = new System.Drawing.Point(404, 0);
            this.pnl_object_list.Name = "pnl_object_list";
            this.pnl_object_list.Size = new System.Drawing.Size(1310, 1039);
            this.pnl_object_list.TabIndex = 3;
            // 
            // pnl_sidebar
            // 
            this.pnl_sidebar.Controls.Add(this.grpbox_console);
            this.pnl_sidebar.Controls.Add(this.grpbox_left_top);
            this.pnl_sidebar.Controls.Add(this.grpbox_bottom_left);
            this.pnl_sidebar.Controls.Add(this.grpbox_load);
            this.pnl_sidebar.Controls.Add(this.btn_match_run);
            this.pnl_sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_sidebar.Location = new System.Drawing.Point(0, 0);
            this.pnl_sidebar.Name = "pnl_sidebar";
            this.pnl_sidebar.Size = new System.Drawing.Size(404, 1039);
            this.pnl_sidebar.TabIndex = 1;
            // 
            // grpbox_console
            // 
            this.grpbox_console.Controls.Add(this.richtextbox_console);
            this.grpbox_console.Location = new System.Drawing.Point(0, 497);
            this.grpbox_console.Name = "grpbox_console";
            this.grpbox_console.Size = new System.Drawing.Size(400, 542);
            this.grpbox_console.TabIndex = 3;
            this.grpbox_console.TabStop = false;
            this.grpbox_console.Text = "Output";
            // 
            // richtextbox_console
            // 
            this.richtextbox_console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richtextbox_console.Location = new System.Drawing.Point(3, 16);
            this.richtextbox_console.Name = "richtextbox_console";
            this.richtextbox_console.ReadOnly = true;
            this.richtextbox_console.Size = new System.Drawing.Size(394, 523);
            this.richtextbox_console.TabIndex = 2;
            this.richtextbox_console.Text = "";
            this.richtextbox_console.TextChanged += new System.EventHandler(this.Richtextbox_console_TextChanged);
            // 
            // grpbox_left_top
            // 
            this.grpbox_left_top.Controls.Add(this.txt_search);
            this.grpbox_left_top.Location = new System.Drawing.Point(3, 3);
            this.grpbox_left_top.Name = "grpbox_left_top";
            this.grpbox_left_top.Size = new System.Drawing.Size(394, 79);
            this.grpbox_left_top.TabIndex = 1;
            this.grpbox_left_top.TabStop = false;
            this.grpbox_left_top.Text = "Search";
            // 
            // txt_search
            // 
            this.txt_search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_search.Location = new System.Drawing.Point(3, 16);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(388, 20);
            this.txt_search.TabIndex = 0;
            this.txt_search.TextChanged += new System.EventHandler(this.Txt_search_TextChanged);
            // 
            // grpbox_bottom_left
            // 
            this.grpbox_bottom_left.Controls.Add(this.btn_save_match);
            this.grpbox_bottom_left.Location = new System.Drawing.Point(3, 373);
            this.grpbox_bottom_left.Name = "grpbox_bottom_left";
            this.grpbox_bottom_left.Size = new System.Drawing.Size(385, 118);
            this.grpbox_bottom_left.TabIndex = 2;
            this.grpbox_bottom_left.TabStop = false;
            this.grpbox_bottom_left.Text = "Save";
            // 
            // btn_save_match
            // 
            this.btn_save_match.Location = new System.Drawing.Point(151, 36);
            this.btn_save_match.Name = "btn_save_match";
            this.btn_save_match.Size = new System.Drawing.Size(100, 50);
            this.btn_save_match.TabIndex = 4;
            this.btn_save_match.Text = "Save Match";
            this.btn_save_match.UseVisualStyleBackColor = true;
            this.btn_save_match.Click += new System.EventHandler(this.Btn_save_match_Click);
            // 
            // grpbox_load
            // 
            this.grpbox_load.Controls.Add(this.btn_browse_match_file_path);
            this.grpbox_load.Controls.Add(this.btn_load_match);
            this.grpbox_load.Controls.Add(this.lbl_card_match_path);
            this.grpbox_load.Controls.Add(this.txt_card_match_path);
            this.grpbox_load.Location = new System.Drawing.Point(3, 168);
            this.grpbox_load.Name = "grpbox_load";
            this.grpbox_load.Size = new System.Drawing.Size(391, 199);
            this.grpbox_load.TabIndex = 2;
            this.grpbox_load.TabStop = false;
            this.grpbox_load.Text = "Load";
            // 
            // btn_browse_match_file_path
            // 
            this.btn_browse_match_file_path.Location = new System.Drawing.Point(310, 61);
            this.btn_browse_match_file_path.Name = "btn_browse_match_file_path";
            this.btn_browse_match_file_path.Size = new System.Drawing.Size(75, 23);
            this.btn_browse_match_file_path.TabIndex = 3;
            this.btn_browse_match_file_path.Text = "Browse";
            this.btn_browse_match_file_path.UseVisualStyleBackColor = true;
            this.btn_browse_match_file_path.Click += new System.EventHandler(this.Btn_browse_match_file_path_Click);
            // 
            // btn_load_match
            // 
            this.btn_load_match.Location = new System.Drawing.Point(151, 117);
            this.btn_load_match.Name = "btn_load_match";
            this.btn_load_match.Size = new System.Drawing.Size(100, 50);
            this.btn_load_match.TabIndex = 2;
            this.btn_load_match.Text = "Load Match";
            this.btn_load_match.UseVisualStyleBackColor = true;
            this.btn_load_match.Click += new System.EventHandler(this.Btn_load_match_Click);
            // 
            // lbl_card_match_path
            // 
            this.lbl_card_match_path.AutoSize = true;
            this.lbl_card_match_path.Location = new System.Drawing.Point(3, 47);
            this.lbl_card_match_path.Name = "lbl_card_match_path";
            this.lbl_card_match_path.Size = new System.Drawing.Size(103, 13);
            this.lbl_card_match_path.TabIndex = 1;
            this.lbl_card_match_path.Text = "Path to matching file";
            // 
            // txt_card_match_path
            // 
            this.txt_card_match_path.Location = new System.Drawing.Point(6, 63);
            this.txt_card_match_path.Name = "txt_card_match_path";
            this.txt_card_match_path.Size = new System.Drawing.Size(298, 20);
            this.txt_card_match_path.TabIndex = 0;
            // 
            // btn_match_run
            // 
            this.btn_match_run.Location = new System.Drawing.Point(154, 88);
            this.btn_match_run.Name = "btn_match_run";
            this.btn_match_run.Size = new System.Drawing.Size(100, 50);
            this.btn_match_run.TabIndex = 1;
            this.btn_match_run.Text = "Auto Match All";
            this.btn_match_run.UseVisualStyleBackColor = true;
            this.btn_match_run.Click += new System.EventHandler(this.Btn_run_Click);
            // 
            // open_file_browse_match_file
            // 
            this.open_file_browse_match_file.Filter = "Json files|*.json|All files|*.*";
            this.open_file_browse_match_file.ShowHelp = true;
            // 
            // ArtworkEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1714, 1039);
            this.Controls.Add(this.pnl_object_list);
            this.Controls.Add(this.pnl_sidebar);
            this.Name = "ArtworkEditor";
            this.Text = "Artwork Editor";
            ((System.ComponentModel.ISupportInitialize)(this.objlist_artwork_editor)).EndInit();
            this.pnl_object_list.ResumeLayout(false);
            this.pnl_sidebar.ResumeLayout(false);
            this.grpbox_console.ResumeLayout(false);
            this.grpbox_left_top.ResumeLayout(false);
            this.grpbox_left_top.PerformLayout();
            this.grpbox_bottom_left.ResumeLayout(false);
            this.grpbox_load.ResumeLayout(false);
            this.grpbox_load.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private BrightIdeasSoftware.ImageRenderer imageRenderer1;
        private System.Windows.Forms.ImageList imageList1;
        private BrightIdeasSoftware.ImageRenderer imageRenderer2;
        private BrightIdeasSoftware.FastObjectListView objlist_artwork_editor;
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
        private System.Windows.Forms.Panel pnl_object_list;
        private System.Windows.Forms.Panel pnl_sidebar;
        private System.Windows.Forms.GroupBox grpbox_console;
        private System.Windows.Forms.RichTextBox richtextbox_console;
        private System.Windows.Forms.GroupBox grpbox_left_top;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.GroupBox grpbox_bottom_left;
        private System.Windows.Forms.Button btn_save_match;
        private System.Windows.Forms.GroupBox grpbox_load;
        private System.Windows.Forms.Button btn_browse_match_file_path;
        private System.Windows.Forms.Button btn_load_match;
        private System.Windows.Forms.Label lbl_card_match_path;
        private System.Windows.Forms.TextBox txt_card_match_path;
        private System.Windows.Forms.Button btn_match_run;
        private System.Windows.Forms.OpenFileDialog open_file_browse_match_file;
    }
}