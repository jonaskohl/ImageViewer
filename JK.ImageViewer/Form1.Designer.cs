namespace JK.ImageViewer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new ToolStrip();
            toolStripButton_openFile = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButton_zoomOut = new ToolStripButton();
            toolStripButton_zoomReset = new ToolStripButton();
            toolStripButton_ZoomIn = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripButton_FolderImgPrev = new ToolStripButton();
            toolStripButton_FolderImgNext = new ToolStripButton();
            openFileDialog1 = new OpenFileDialog();
            imageViewControl1 = new ImageViewControl();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openImageToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            previousImageToolStripMenuItem = new ToolStripMenuItem();
            nextImageToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            closeToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            zoominToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            zoomoutToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton_openFile, toolStripSeparator1, toolStripButton_zoomOut, toolStripButton_zoomReset, toolStripButton_ZoomIn, toolStripSeparator2, toolStripButton_FolderImgPrev, toolStripButton_FolderImgNext });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_openFile
            // 
            toolStripButton_openFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_openFile.Image = Properties.Resources.folder_document;
            toolStripButton_openFile.Name = "toolStripButton_openFile";
            toolStripButton_openFile.Size = new Size(23, 22);
            toolStripButton_openFile.Text = "Open file...";
            toolStripButton_openFile.Click += toolStripButton_openFile_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripButton_zoomOut
            // 
            toolStripButton_zoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_zoomOut.Image = Properties.Resources.zoom_out;
            toolStripButton_zoomOut.Name = "toolStripButton_zoomOut";
            toolStripButton_zoomOut.Size = new Size(23, 22);
            toolStripButton_zoomOut.Text = "Zoom out";
            toolStripButton_zoomOut.Click += toolStripButton_zoomOut_Click;
            // 
            // toolStripButton_zoomReset
            // 
            toolStripButton_zoomReset.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_zoomReset.Image = Properties.Resources.view_1_1;
            toolStripButton_zoomReset.Name = "toolStripButton_zoomReset";
            toolStripButton_zoomReset.Size = new Size(23, 22);
            toolStripButton_zoomReset.Text = "Original size";
            toolStripButton_zoomReset.Click += toolStripButton_zoomReset_Click;
            // 
            // toolStripButton_ZoomIn
            // 
            toolStripButton_ZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_ZoomIn.Image = Properties.Resources.zoom_in;
            toolStripButton_ZoomIn.Name = "toolStripButton_ZoomIn";
            toolStripButton_ZoomIn.Size = new Size(23, 22);
            toolStripButton_ZoomIn.Text = "Zoom in";
            toolStripButton_ZoomIn.Click += toolStripButton_ZoomIn_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripButton_FolderImgPrev
            // 
            toolStripButton_FolderImgPrev.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_FolderImgPrev.Image = Properties.Resources.nav_left;
            toolStripButton_FolderImgPrev.Name = "toolStripButton_FolderImgPrev";
            toolStripButton_FolderImgPrev.Size = new Size(23, 22);
            toolStripButton_FolderImgPrev.Text = "Previous image in folder";
            toolStripButton_FolderImgPrev.Click += toolStripButton_FolderImgPrev_Click;
            // 
            // toolStripButton_FolderImgNext
            // 
            toolStripButton_FolderImgNext.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_FolderImgNext.Image = Properties.Resources.nav_right;
            toolStripButton_FolderImgNext.Name = "toolStripButton_FolderImgNext";
            toolStripButton_FolderImgNext.Size = new Size(23, 22);
            toolStripButton_FolderImgNext.Text = "Next image in folder";
            toolStripButton_FolderImgNext.Click += toolStripButton_FolderImgNext_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // imageViewControl1
            // 
            imageViewControl1.Dock = DockStyle.Fill;
            imageViewControl1.Location = new Point(0, 49);
            imageViewControl1.Name = "imageViewControl1";
            imageViewControl1.Size = new Size(800, 401);
            imageViewControl1.TabIndex = 1;
            imageViewControl1.Text = "imageViewControl1";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openImageToolStripMenuItem, toolStripSeparator3, previousImageToolStripMenuItem, nextImageToolStripMenuItem, toolStripSeparator4, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // openImageToolStripMenuItem
            // 
            openImageToolStripMenuItem.Image = Properties.Resources.folder_document;
            openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            openImageToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openImageToolStripMenuItem.Size = new Size(193, 22);
            openImageToolStripMenuItem.Text = "&Open image...";
            openImageToolStripMenuItem.Click += openImageToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(190, 6);
            // 
            // previousImageToolStripMenuItem
            // 
            previousImageToolStripMenuItem.Image = Properties.Resources.nav_left;
            previousImageToolStripMenuItem.Name = "previousImageToolStripMenuItem";
            previousImageToolStripMenuItem.Size = new Size(193, 22);
            previousImageToolStripMenuItem.Text = "&Previous image";
            previousImageToolStripMenuItem.Click += previousImageToolStripMenuItem_Click;
            // 
            // nextImageToolStripMenuItem
            // 
            nextImageToolStripMenuItem.Image = Properties.Resources.nav_right;
            nextImageToolStripMenuItem.Name = "nextImageToolStripMenuItem";
            nextImageToolStripMenuItem.Size = new Size(193, 22);
            nextImageToolStripMenuItem.Text = "&Next image";
            nextImageToolStripMenuItem.Click += nextImageToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(190, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            closeToolStripMenuItem.Size = new Size(193, 22);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoomToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "&View";
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoominToolStripMenuItem, resetToolStripMenuItem, zoomoutToolStripMenuItem });
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new Size(106, 22);
            zoomToolStripMenuItem.Text = "&Zoom";
            // 
            // zoominToolStripMenuItem
            // 
            zoominToolStripMenuItem.Image = Properties.Resources.zoom_in;
            zoominToolStripMenuItem.Name = "zoominToolStripMenuItem";
            zoominToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Oemplus;
            zoominToolStripMenuItem.Size = new Size(222, 22);
            zoominToolStripMenuItem.Text = "Zoom &in";
            zoominToolStripMenuItem.Click += zoominToolStripMenuItem_Click;
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Image = Properties.Resources.view_1_1;
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D0;
            resetToolStripMenuItem.Size = new Size(222, 22);
            resetToolStripMenuItem.Text = "&Reset";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // zoomoutToolStripMenuItem
            // 
            zoomoutToolStripMenuItem.Image = Properties.Resources.zoom_out;
            zoomoutToolStripMenuItem.Name = "zoomoutToolStripMenuItem";
            zoomoutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.OemMinus;
            zoomoutToolStripMenuItem.Size = new Size(222, 22);
            zoomoutToolStripMenuItem.Text = "Zoom &out";
            zoomoutToolStripMenuItem.Click += zoomoutToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(imageViewControl1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Image Viewer";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton_openFile;
        private OpenFileDialog openFileDialog1;
        private ImageViewControl imageViewControl1;
        private ToolStripButton toolStripButton_zoomOut;
        private ToolStripButton toolStripButton_zoomReset;
        private ToolStripButton toolStripButton_ZoomIn;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButton_FolderImgPrev;
        private ToolStripButton toolStripButton_FolderImgNext;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openImageToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem zoominToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem zoomoutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem previousImageToolStripMenuItem;
        private ToolStripMenuItem nextImageToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem closeToolStripMenuItem;
    }
}
