namespace ChromeBookmarker
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cbTreeView = new System.Windows.Forms.TreeView();
            this.iconImgList = new System.Windows.Forms.ImageList(this.components);
            this.pnlDropWindowPanel = new System.Windows.Forms.Panel();
            this.lblDragWindowCaption = new System.Windows.Forms.Label();
            this.cmsBookmark = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.lActiveBookmarkLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.saveTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlDropWindowPanel.SuspendLayout();
            this.cmsBookmark.SuspendLayout();
            this.cmsFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTreeView
            // 
            this.cbTreeView.AllowDrop = true;
            this.cbTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbTreeView.HideSelection = false;
            this.cbTreeView.ImageIndex = 0;
            this.cbTreeView.ImageList = this.iconImgList;
            this.cbTreeView.Location = new System.Drawing.Point(2, 25);
            this.cbTreeView.Name = "cbTreeView";
            this.cbTreeView.SelectedImageIndex = 0;
            this.cbTreeView.Size = new System.Drawing.Size(502, 179);
            this.cbTreeView.TabIndex = 0;
            this.cbTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.cbTreeView_ItemDrag);
            this.cbTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.cbTreeView_AfterSelect);
            this.cbTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.cbTreeView_NodeMouseClick);
            this.cbTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbTreeView_DragDrop);
            this.cbTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbTreeView_DragEnter);
            // 
            // iconImgList
            // 
            this.iconImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconImgList.ImageStream")));
            this.iconImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconImgList.Images.SetKeyName(0, "folder.png");
            this.iconImgList.Images.SetKeyName(1, "blockdevice.png");
            // 
            // pnlDropWindowPanel
            // 
            this.pnlDropWindowPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDropWindowPanel.Controls.Add(this.lblDragWindowCaption);
            this.pnlDropWindowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDropWindowPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlDropWindowPanel.Name = "pnlDropWindowPanel";
            this.pnlDropWindowPanel.Size = new System.Drawing.Size(506, 23);
            this.pnlDropWindowPanel.TabIndex = 4;
            // 
            // lblDragWindowCaption
            // 
            this.lblDragWindowCaption.AutoSize = true;
            this.lblDragWindowCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblDragWindowCaption.ForeColor = System.Drawing.Color.White;
            this.lblDragWindowCaption.Location = new System.Drawing.Point(10, 5);
            this.lblDragWindowCaption.Name = "lblDragWindowCaption";
            this.lblDragWindowCaption.Size = new System.Drawing.Size(135, 13);
            this.lblDragWindowCaption.TabIndex = 0;
            this.lblDragWindowCaption.Text = "lblDragWindowCaption";
            // 
            // cmsBookmark
            // 
            this.cmsBookmark.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.cmsBookmark.Name = "cmsBookmark";
            this.cmsBookmark.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // cmsFolder
            // 
            this.cmsFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1,
            this.editToolStripMenuItem1,
            this.newFolderToolStripMenuItem});
            this.cmsFolder.Name = "cmsFolder";
            this.cmsFolder.Size = new System.Drawing.Size(133, 70);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.editToolStripMenuItem1.Text = "Edit";
            this.editToolStripMenuItem1.Click += new System.EventHandler(this.editToolStripMenuItem1_Click);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.newFolderToolStripMenuItem.Text = "New folder";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // pbPreview
            // 
            this.pbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(2, 226);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(502, 154);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 5;
            this.pbPreview.TabStop = false;
            // 
            // lActiveBookmarkLink
            // 
            this.lActiveBookmarkLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lActiveBookmarkLink.Location = new System.Drawing.Point(40, 207);
            this.lActiveBookmarkLink.Name = "lActiveBookmarkLink";
            this.lActiveBookmarkLink.Size = new System.Drawing.Size(462, 16);
            this.lActiveBookmarkLink.TabIndex = 6;
            this.lActiveBookmarkLink.TabStop = true;
            this.lActiveBookmarkLink.Text = "linkLabel1";
            this.lActiveBookmarkLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lActiveBookmarkLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "URL:";
            // 
            // saveTimer
            // 
            this.saveTimer.Interval = 60000;
            this.saveTimer.Tick += new System.EventHandler(this.saveTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(506, 383);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lActiveBookmarkLink);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.pnlDropWindowPanel);
            this.Controls.Add(this.cbTreeView);
            this.MinimumSize = new System.Drawing.Size(377, 190);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chrome Bookmarker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlDropWindowPanel.ResumeLayout(false);
            this.pnlDropWindowPanel.PerformLayout();
            this.cmsBookmark.ResumeLayout(false);
            this.cmsFolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView cbTreeView;
        private System.Windows.Forms.Panel pnlDropWindowPanel;
        private System.Windows.Forms.Label lblDragWindowCaption;
        private System.Windows.Forms.ContextMenuStrip cmsBookmark;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsFolder;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ImageList iconImgList;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.LinkLabel lActiveBookmarkLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer saveTimer;
    }
}

