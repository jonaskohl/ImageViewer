using JK.ImageViewer.Controls;

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
            exitToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            zoominToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            zoomoutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            fitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel_ImageResoultion = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // imageViewControl1
            // 
            imageViewControl1.Dock = DockStyle.Fill;
            imageViewControl1.Location = new Point(0, 49);
            imageViewControl1.Name = "imageViewControl1";
            imageViewControl1.Size = new Size(800, 379);
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
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openImageToolStripMenuItem, toolStripSeparator3, previousImageToolStripMenuItem, nextImageToolStripMenuItem, toolStripSeparator4, closeToolStripMenuItem, exitToolStripMenuItem });
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
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
            exitToolStripMenuItem.Size = new Size(193, 22);
            exitToolStripMenuItem.Text = "&Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
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
            zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoominToolStripMenuItem, resetToolStripMenuItem, zoomoutToolStripMenuItem, toolStripSeparator5, fitToolStripMenuItem });
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
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(219, 6);
            // 
            // fitToolStripMenuItem
            // 
            fitToolStripMenuItem.Name = "fitToolStripMenuItem";
            fitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D1;
            fitToolStripMenuItem.Size = new Size(222, 22);
            fitToolStripMenuItem.Text = "Fit";
            fitToolStripMenuItem.Click += fitToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_ImageResoultion });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_ImageResoultion
            // 
            toolStripStatusLabel_ImageResoultion.Name = "toolStripStatusLabel_ImageResoultion";
            toolStripStatusLabel_ImageResoultion.Size = new Size(0, 17);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(imageViewControl1);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Image Viewer";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private OpenFileDialog openFileDialog1;
        private ImageViewControl imageViewControl1;
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
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel_ImageResoultion;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem fitToolStripMenuItem;
    }
}
