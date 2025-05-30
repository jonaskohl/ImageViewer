using JK.ImageViewer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class Form1
    {
        [WindowCommand("Open file...", MenuLocation = "File")]
        public void Command_OpenFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                LoadImage(openFileDialog1.FileName);
        }

        [WindowCommand("Zoom in")]
        public void Command_ZoomIn()
        {
            SetZoomFactor(Math.Min(imageViewControl1.ZoomFactor + 0.125f, 8f));
        }

        [WindowCommand("Reset zoom")]
        public void Command_ZoomOriginal()
        {
            SetZoomFactor(1f);
        }

        [WindowCommand("Zoom out")]
        public void Command_ZoomOut()
        {
            SetZoomFactor(Math.Max(imageViewControl1.ZoomFactor - 0.125f, 0.125f));
        }

        [WindowCommand("Previous image in folder")]
        public void Command_FolderImagePrevious()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex - 1 + folderFiles!.Length) % folderFiles.Length;
            LoadImage(folderFiles[folderIndex]);
        }

        [WindowCommand("Next image in folder")]
        public void Command_FolderImageNext()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex + 1) % folderFiles!.Length;
            LoadImage(folderFiles[folderIndex]);
        }

        [WindowCommand("Close")]
        public void Command_CloseImage()
        {
            ClearImage(true);
        }

        [WindowCommand("Exit")]
        public void Command_ExitApplication()
        {
            Close();
        }

        [WindowCommand("Zoom to fit")]
        public void Command_ZoomToFit()
        {
            SetZoomFactor(GetBestFitZoomFactor());
        }
    }
}
