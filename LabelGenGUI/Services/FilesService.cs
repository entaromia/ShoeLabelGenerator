﻿using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LabelGenGUI.Services
{
    public class FilesService(TopLevel target)
    {
        private readonly IStorageProvider storageProvider = target.StorageProvider;
        private readonly FilePickerFileType filePickerFileTypeJson = new("Proje Dosyası") { Patterns = ["*.json"], MimeTypes = ["application/json"] };

        public async Task<IStorageFile?> GetSaveFileAsync()
        {
            if (storageProvider.CanSave)
            {
                var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
                { Title = "Proje dosyasını seçin", FileTypeChoices = [filePickerFileTypeJson], SuggestedFileName = "proje", DefaultExtension = ".json", ShowOverwritePrompt = true });
                return file ?? null;
            }
            else
            {
                throw new PlatformNotSupportedException("File saving is not supported on: " + RuntimeInformation.OSDescription);
            }
        }

        public async Task<IStorageFile?> GetOpenFileAsync()
        {
            if (storageProvider.CanOpen)
            {
                var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                { Title = "Açılacak projeyi seçin", FileTypeFilter = [filePickerFileTypeJson] });
                return files?.Count >= 1 ? files[0] : null;
            }
            else
            {
                throw new PlatformNotSupportedException("File opening is not supported on: " + RuntimeInformation.OSDescription);
            }
        }

        public async Task<string?> GetFolderPathAsync()
        {
            if (storageProvider.CanPickFolder)
            {
                var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { Title = "Klasör seçin" });
                return folders?.Count >= 1 ? folders[0].Path.AbsolutePath : null;
            }
            else
            {
                throw new PlatformNotSupportedException("Folder picking is not supported on: " + RuntimeInformation.OSDescription);
            }
        }

    }
}
