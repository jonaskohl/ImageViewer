
using ImageMagick;
using System.Diagnostics;

namespace JK.ImageViewer
{
    public partial class Form1 : Form
    {
        string baseTitle;
        string? currentPath;

        string[]? folderFiles = null;
        int folderIndex = -1;


        public Form1()
        {
            InitializeComponent();
            baseTitle = Text;

            imageViewControl1.ShowCheckerboard = true;
        }

        private void toolStripButton_openFile_Click(object sender, EventArgs e)
        {
            Command_OpenFile();
        }

        private void toolStripButton_ZoomIn_Click(object sender, EventArgs e)
        {
            Command_ZoomIn();
        }

        private void toolStripButton_zoomReset_Click(object sender, EventArgs e)
        {
            Command_ZoomOriginal();
        }

        private void toolStripButton_zoomOut_Click(object sender, EventArgs e)
        {
            Command_ZoomOut();
        }

        private void toolStripButton_FolderImgPrev_Click(object sender, EventArgs e)
        {
            Command_FolderImagePrevious();
        }

        private void toolStripButton_FolderImgNext_Click(object sender, EventArgs e)
        {
            Command_FolderImageNext();
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_OpenFile();
        }

        private void zoominToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomIn();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomOriginal();
        }

        private void zoomoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_ZoomOut();
        }

        private void previousImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_FolderImagePrevious();
        }

        private void nextImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_FolderImageNext();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command_CloseApplication();
        }
    }
}
