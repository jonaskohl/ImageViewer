﻿using JK.ImageViewer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer
{
    public partial class MainForm
    {
        [WindowCommand]
        public void Command_OpenFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                if (openFileDialog1.FileNames.Length == 1)
                    LoadImage(openFileDialog1.FileName);
                else
                    LoadImage(openFileDialog1.FileName, openFileDialog1.FileNames);
        }

        [WindowCommand]
        public void Command_ZoomIn()
        {
            SetZoomFactor(imageViewControl1.ZoomFactor + 0.125f);
        }

        [WindowCommand]
        public void Command_ZoomOriginal()
        {
            SetZoomFactor(1f);
        }

        [WindowCommand]
        public void Command_ZoomOut()
        {
            SetZoomFactor(imageViewControl1.ZoomFactor - 0.125f);
        }

        [WindowCommand]
        public void Command_FolderImagePrevious()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex - 1 + folderFiles!.Length) % folderFiles.Length;
            LoadImage(folderFiles[folderIndex], folderFiles);
        }

        [WindowCommand]
        public void Command_FolderImageNext()
        {
            if (!TryFetchFolder())
                return;

            folderIndex = (folderIndex + 1) % folderFiles!.Length;
            LoadImage(folderFiles[folderIndex], folderFiles);
        }

        [WindowCommand]
        public void Command_CloseImage()
        {
            ClearImage(true);
        }

        [WindowCommand]
        public void Command_ExitApplication()
        {
            Close();
        }

        [WindowCommand]
        public void Command_ZoomToFit()
        {
            SetZoomFactor(GetBestFitZoomFactor());
        }

        [WindowCommand]
        public void Command_ShowSettings()
        {
            using var diag = new SettingsForm();
            diag.ShowDialog(this);
        }

        [WindowCommand]
        public void Command_SaveFile()
        {
            // TODO
            SetUnsaved(false);
        }
    }
}
