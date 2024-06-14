using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ImageSharpLabelGen.Helpers;
using LabelGenGUI.Services;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private static FilePickerFileType FilePickerFileTypeJson => new("Proje Dosyası") { Patterns = ["*.json"], MimeTypes = ["application/json"] };
    private ParcelAndBoxHelper? parcelAndBoxHelper = null;

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

    private static void NavigateTo(NavigationService.ViewModelName name)
    {
        NavigationService.Instance.Navigate(name);
    }

    // Avalonia throws when static methods are used as commands
    public bool CanSaveProject(object msg) => ShoeListService.Instance.CurrentFile is not null;
    public bool CanCloseProject(object msg) => ShoeListService.Instance.CurrentFile is not null;
    public bool CanSaveAsPicture(object msg) => ShoeListService.Instance.ItemCount > 0;
    public bool CanPrint(object msg) => ShoeListService.Instance.ItemCount > 0;

    public async Task SaveProject()
    {
        if (ShoeListService.Instance.CurrentFile is null)
        {
            throw new NullReferenceException("Missing current file path");
        }
        await ShoeListService.Instance.SaveToFileAsync();
    }

    public async Task NewProject()
    {
        var file = await GetSaveFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
            ShoeListService.Instance.ProjectName = file.Name[..file.Name.IndexOf(".json")];
            if (!NavigationService.Instance.ContentHasPage)
                NavigateTo(NavigationService.ViewModelName.ShoeListViewModel);
        }
    }

    public async Task OpenProject()
    {
        var file = await GetOpenFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
            await ShoeListService.Instance.OpenFileAsync();
            if (!NavigationService.Instance.ContentHasPage)
                NavigateTo(NavigationService.ViewModelName.ShoeListViewModel);
        }
    }

    public void CloseProject()
    {
        NavigationService.Instance.MainPage();
        ShoeListService.Instance.CloseProject();
    }

    // TODO: Implement actual printing
    public async Task Print()
    {

    }

    public async Task SaveAsPicture()
    {
        // Initialize parcel and box helper on first save
        parcelAndBoxHelper ??= new();

        parcelAndBoxHelper.OutputFolder = await GetFolderPathAsync();
        if (parcelAndBoxHelper.OutputFolder is not null)
        {
            await parcelAndBoxHelper.WriteParcelAndBoxAsync(ShoeListService.Instance.GetItems());
        }
    }
#pragma warning restore CA1822 // Mark members as static
}