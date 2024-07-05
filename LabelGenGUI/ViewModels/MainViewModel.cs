using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using ImageSharpLabelGen.Helpers;
using LabelGenGUI.Services;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private static FilePickerFileType FilePickerFileTypeJson => new("Proje Dosyası") { Patterns = ["*.json"], MimeTypes = ["application/json"] };

    private static async Task<IStorageFile?> GetSaveFileAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
        if (provider.CanSave)
        {
            var file = await provider.SaveFilePickerAsync(new FilePickerSaveOptions()
            { Title = "Proje dosyasını seçin", FileTypeChoices = [FilePickerFileTypeJson], DefaultExtension = ".json", ShowOverwritePrompt = true });
            return file ?? null;
        }
        else
        {
            throw new PlatformNotSupportedException("File saving is not supported on: " + RuntimeInformation.OSDescription);
        }
    }

    private static async Task<IStorageFile?> GetOpenFileAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
        if (provider.CanOpen)
        {
            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            { Title = "Açılacak projeyi seçin", FileTypeFilter = [FilePickerFileTypeJson] });
            return files?.Count >= 1 ? files[0] : null;
        }
        else
        {
            throw new PlatformNotSupportedException("File opening is not supported on: " + RuntimeInformation.OSDescription);
        }
    }

    private static async Task<string?> GetFolderPathAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
        if (provider.CanPickFolder)
        {
            var folders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { Title = "Klasör seçin"});
            return folders?.Count >= 1 ? folders[0].Path.AbsolutePath : null;
        }
        else
        {
            throw new PlatformNotSupportedException("Folder picking is not supported on: " + RuntimeInformation.OSDescription);
        }
    }

    private static void NavigateTo(NavigationService.Pages name)
    {
        NavigationService.Instance.Navigate(name);
    }

    // Avalonia throws when static methods are used as commands
    private bool ProjectOpen() => ShoeListService.Instance.CurrentFile is not null;
    private bool CanSave() => ShoeListService.Instance.ItemCount > 0;

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private async Task SaveProject()
    {
        if (ShoeListService.Instance.CurrentFile is null)
        {
            throw new NullReferenceException("Missing current file path");
        }
        await ShoeListService.Instance.SaveToFileAsync();
    }

    [RelayCommand]
    private async Task NewProject()
    {
        var file = await GetSaveFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
            ShoeListService.Instance.ProjectName = file.Name[..file.Name.IndexOf(".json")];
            if (!NavigationService.Instance.ContentHasPage)
                GoToListView();
        }
    }

    [RelayCommand]
    private async Task OpenProject()
    {
        var file = await GetOpenFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
            await ShoeListService.Instance.OpenFileAsync();
            if (!NavigationService.Instance.ContentHasPage)
                GoToListView();
        }
    }

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private void CloseProject()
    {
        NavigationService.Instance.MainPage();
        ShoeListService.Instance.CloseProject();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Print()
    {
        NavigateTo(NavigationService.Pages.PrintPage);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsPicture()
    {
        var folder = await GetFolderPathAsync();
        if (folder is not null)
        {
            LabelImageSaveHelper.SaveToPng(ShoeListService.Instance.GetItems(), folder);
        }
    }

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private void GoToListView() => NavigateTo(NavigationService.Pages.ShoeListPage);

    [RelayCommand]
    private void GoBack() => NavigationService.Instance.NavigateBack();
}