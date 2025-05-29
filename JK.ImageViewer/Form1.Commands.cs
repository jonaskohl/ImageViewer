using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class Form1
    {
        public void Command_OpenFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                LoadImage(openFileDialog1.FileName);
        }

        public void Command_ZoomIn()
        {
            imageViewControl1.ZoomFactor = Math.Min(imageViewControl1.ZoomFactor + 0.125f, 8f);
        }

        public void Command_ZoomOriginal()
        {
            imageViewControl1.ZoomFactor = 1f;
        }

        public void Command_ZoomOut()
        {
            imageViewControl1.ZoomFactor = Math.Max(imageViewControl1.ZoomFactor - 0.125f, 0.125f);
        }

        public void Command_FolderImagePrevious()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex - 1 + folderFiles.Length) % folderFiles.Length;
            LoadImage(folderFiles[folderIndex]);
        }

        public void Command_FolderImageNext()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex + 1) % folderFiles.Length;
            LoadImage(folderFiles[folderIndex]);
        }

        public void Command_CloseApplication()
        {
            Close();
        }
    }
}
