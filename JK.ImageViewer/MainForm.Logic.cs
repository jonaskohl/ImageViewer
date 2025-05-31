using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class MainForm
    {
        private string? GetCurrentFolderPath()
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

            ClearImage();
            try
            {
                using var magickImage = new MagickImage(path);

                imageViewControl1.ContentImage = magickImage.ToBitmap();

                UpdateZoomText();
            }
            catch (MagickDelegateErrorException ex)
            {
                imageViewControl1.ImageLoadException = ex;
            }

            Text = Path.GetFileName(path) + " - " + baseTitle;
            currentPath = path;

            Application.DoEvents();
            Application.UseWaitCursor = false;
            Cursor.Position = Cursor.Position;
        }

        private void ClearImage(bool clearFolderPosition = false)
        {
            imageViewControl1.ImageLoadException = null;
            imageViewControl1.ContentImage?.Dispose();
            imageViewControl1.ContentImage = null!;
            Text = baseTitle;
            currentPath = null;
            UpdateZoomText();

            if (clearFolderPosition)
            {
                folderIndex = -1;
                folderFiles = null;
            }
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
            zoomFactor = MathUtil.Clamp(0.125f, zoomFactor, 8f);

            imageViewControl1.ZoomFactor = zoomFactor;
        }

        private float GetBestFitZoomFactor()
        {
            if (imageViewControl1.ContentImage is null)
                return 1f;

            return Math.Min(
                imageViewControl1.ClientSize.Width / (float)imageViewControl1.ContentImage.Width,
                imageViewControl1.ClientSize.Height / (float)imageViewControl1.ContentImage.Height
            );
        }

        private void UpdateZoomText()
        {
            if (imageViewControl1.ContentImage is null)
                toolStripStatusLabel_ImageResoultion.Text = string.Empty;
            else
                toolStripStatusLabel_ImageResoultion.Text =
                    $"{imageViewControl1.ContentImage.Width}x{imageViewControl1.ContentImage.Height} \u2192 " +
                    $"{Math.Round(imageViewControl1.ContentImage.Width * imageViewControl1.ZoomFactor)}x{Math.Round(imageViewControl1.ContentImage.Height * imageViewControl1.ZoomFactor)}";
        }
    }
}
