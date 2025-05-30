using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class Form1
    {        private string? GetCurrentFolderPath()
        {
            if (currentPath is null)
                return null;

            return Path.GetDirectoryName(currentPath);
        }

        private string[] GetImagesInFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(item => IsFileSupported(item))
                .ToArray();
        }

        private bool IsFileSupported(string path)
        {
            var ext = Path.GetExtension(path)?.ToLowerInvariant().Substring(1);
            if (ext is null)
                return false;
            return MagickNET.SupportedFormats
                .Any(format => format.SupportsReading && format.Format.ToString().Equals(ext, StringComparison.OrdinalIgnoreCase));
        }

        public void LoadImage(string path)
        {
            Application.UseWaitCursor = true;
            Application.DoEvents();
            Cursor.Position = Cursor.Position;
            Application.DoEvents();

            try
            {
                using var magickImage = new MagickImage(path);

                ClearImage();
                imageViewControl1.ContentImage = magickImage.ToBitmap();

                Text = Path.GetFileName(path) + " - " + baseTitle;
                currentPath = path;
            } catch (MagickDelegateErrorException ex) {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.DoEvents();
            Application.UseWaitCursor = false;
            Cursor.Position = Cursor.Position;
        }

        private void ClearImage()
        {
            imageViewControl1.ContentImage?.Dispose();
            imageViewControl1.ContentImage = null!;
            Text = baseTitle;
            currentPath = null;
        }

        private bool TryFetchFolder()
        {
            if (folderFiles is null)
            {
                var folderPath = GetCurrentFolderPath();
                if (folderPath is null)
                    return false;
                folderFiles = GetImagesInFolder(folderPath);

                folderIndex = Array.IndexOf(folderFiles, currentPath);
            }

            return true;
        }

        private void SetZoomFactor(float zoomFactor)
        {
            imageViewControl1.ZoomFactor = zoomFactor;
            toolStripNumericUpDown1.Value = (decimal)zoomFactor * 100m;
        }
    }
}
