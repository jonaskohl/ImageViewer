using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class MainForm
    {
        private void SetUnsaved(bool unsaved)
        {
            isUnsaved = unsaved;
            SetCommandEnabled("SaveFile", unsaved);
        }

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

        public void LoadImage(string path, string[]? folderFiles = null)
        {
            Application.UseWaitCursor = true;
            Application.DoEvents();
            Cursor.Position = Cursor.Position;
            Application.DoEvents();

            try
            {
                if (!ClearImage(folderFiles is null))
                    return;

                try
                {
                    using var magickImage = new MagickImage();
                    magickImage.BackgroundColor = MagickColors.Transparent;
                    magickImage.Read(path);

                    imageViewControl1.ContentImage = magickImage.ToBitmap();

                    this.folderFiles = folderFiles;
                    if (folderFiles is not null)
                        folderIndex = Array.IndexOf(folderFiles, path);
                } catch (Exception ex)
                {
                    imageViewControl1.ImageLoadException = ex;
                }

                UpdateZoomText();

                SetCommandEnabled("ZoomIn", true);
                SetCommandEnabled("ZoomOut", true);
                SetCommandEnabled("ZoomOriginal", true);
                SetCommandEnabled("ZoomToFit", true);
                SetCommandEnabled("CloseImage", true);
                SetCommandEnabled("FolderImagePrevious", true);
                SetCommandEnabled("FolderImageNext", true);
                SetToolEnabled(EditorTool.Default, true);
                SetToolEnabled(EditorTool.Zoom, true);
                SetToolEnabled(EditorTool.Crop, true);
                SetToolEnabled(EditorTool.Rectangle, true);
                SetToolEnabled(EditorTool.Ellipse, true);
                SetToolEnabled(EditorTool.Line, true);
                SetEnabled(zoomInput, true);
                SetEnabled(colorPicker, true);

                Text = Path.GetFileName(path) + " - " + baseTitle;
                currentPath = path;
            }
            finally
            {
                Application.DoEvents();
                Application.UseWaitCursor = false;
                Cursor.Position = Cursor.Position;
            }
        }

        private void ReplaceCurrentImage(Image? image)
        {
            imageViewControl1.ImageLoadException = null;
            imageViewControl1.ContentImage?.Dispose();
            imageViewControl1.ContentImage = image;
        }

        private bool ClearImage(bool clearFolderPosition = false)
        {
            if (isUnsaved)
            {
                var saveButton = new TaskDialogCommandLinkButton()
                {
                    Text = this._("Dialogs.UnsavedChanges.Buttons.Save.Label"),
                    DescriptionText = this._("Dialogs.UnsavedChanges.Buttons.Save.Description"),
                };
                var discardButton = new TaskDialogCommandLinkButton()
                {
                    Text = this._("Dialogs.UnsavedChanges.Buttons.Discard.Label"),
                    DescriptionText = this._("Dialogs.UnsavedChanges.Buttons.Discard.Description"),
                };
                var cancelButton = new TaskDialogCommandLinkButton()
                {
                    Text = this._("Dialogs.UnsavedChanges.Buttons.Cancel.Label"),
                    DescriptionText = this._("Dialogs.UnsavedChanges.Buttons.Cancel.Description"),
                };

                var result = TaskDialog.ShowDialog(Handle, new TaskDialogPage()
                {
                    AllowCancel = true,
                    AllowMinimize = false,
                    Icon = TaskDialogIcon.Warning,
                    Buttons =
                    {
                        saveButton,
                        discardButton,
                        cancelButton,
                    },
                    Caption = this._("Dialogs.UnsavedChanges.Title"),
                    Heading = this._("Dialogs.UnsavedChanges.Title"),
                    Text = string.Format(this._("Dialogs.UnsavedChanges.Message"), Path.GetFileName(currentPath)),
                });
                if (result == saveButton)
                {
                    Command_SaveFile();
                }
                else if (result == cancelButton || result == TaskDialogButton.Cancel)
                {
                    return false;
                }
            }

            ReplaceCurrentImage(null);
            Text = baseTitle;
            currentPath = null;
            SetUnsaved(false);
            UpdateZoomText();

            SetCommandEnabled("SaveFile", false);
            SetCommandEnabled("ZoomIn", false);
            SetCommandEnabled("ZoomOut", false);
            SetCommandEnabled("ZoomOriginal", false);
            SetCommandEnabled("ZoomToFit", false);
            SetCommandEnabled("CloseImage", false);
            SetToolEnabled(EditorTool.Default, false);
            SetToolEnabled(EditorTool.Zoom, false);
            SetToolEnabled(EditorTool.Crop, false);
            SetToolEnabled(EditorTool.Rectangle, false);
            SetToolEnabled(EditorTool.Ellipse, false);
            SetToolEnabled(EditorTool.Line, false);
            SetEnabled(zoomInput, false);
            SetEnabled(colorPicker, false);

            if (clearFolderPosition)
            {
                folderIndex = -1;
                folderFiles = null;

                SetCommandEnabled("FolderImagePrevious", false);
                SetCommandEnabled("FolderImageNext", false);
            }

            return true;
        }

        public void SetEnabled(ToolStripItem? control, bool enabled)
        {
            if (control is null)
                return;

            control.Enabled = enabled;
        }

        public void SetEnabled(Control? control, bool enabled)
        {
            if (control is null)
                return;

            control.Enabled = enabled;
        }

        private void SetCommandEnabled(string commandName, bool isEnabled)
        {
            if (commandButtons.TryGetValue(commandName, out var button))
                button.Enabled = isEnabled;

            if (commandMenuItems.TryGetValue(commandName, out var item))
                item.Enabled = isEnabled;

            commandEnabledState[commandName] = isEnabled;
        }

        private void SetToolEnabled(EditorTool tool, bool isEnabled)
        {
            if (toolButtons.TryGetValue(tool, out var button))
                button.Enabled = isEnabled;

            toolEnabledState[tool] = isEnabled;
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
            zoomFactor = MathUtil.Clamp(Constants.ZOOM_FACTOR_MIN,  zoomFactor, Constants.ZOOM_FACTOR_MAX);

            imageViewControl1.ZoomFactor = zoomFactor;
        }

        private float GetBestFitZoomFactor()
        {
            return imageViewControl1.GetBestFitZoomFactor();
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
