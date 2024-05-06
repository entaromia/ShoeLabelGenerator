using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ImageSharpLabelGen;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LabelGenGUI.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ParcelAndBoxHelper parcelBoxHelper;

    private int total;
    public int Total
    {
        get { return total; }
        set
        {
            total = value;
            PropertyChangedEvent();
        }
    }

    // User selection of the brand
    public string[] Brands { get; } = [
        "RIDGE",
        "GERONIMO"
        ];
    public string? SelectedBrand { get; set; }

    // User selection of different qualities
    public string[] Qualities { get; } = [
        "SRC 01",
        "SRC 02",
        "LFR 01",
        "LFR 02"
        ];

    public string? SelectedQuality { get; set; }

    public string? Color { get; set; }

    public string? ReceiptNo { get; set; }

    // We support 8 different shoe pairs for now
    public ObservableCollection<int> ShoeCounts { get; set; } = [0, 0, 0, 0, 0, 0, 0, 0];

    public MainViewModel()
    {
        parcelBoxHelper = new ParcelAndBoxHelper();

        // update total shoe count on shoe count input changes
        ShoeCounts.CollectionChanged += (_, _) =>
        {
            int total = 0;
            foreach (var count in ShoeCounts)
            {
                total += count;
            }
            Total = total;
        };
    }

    public void WriteBoxAndParcel()
    {
        if (parcelBoxHelper.OutputFolder is null)
        {
            ShowDialog(ErrorMessage.FolderNotPicked);
            return;
        }
        try
        {
            parcelBoxHelper.WriteParcelAndBox(ShoeCounts, SelectedBrand, SelectedQuality, Color, ReceiptNo);
        }
        catch (ArgumentNullException)
        {
            ShowDialog(ErrorMessage.EmptyInputFields);
        }
        catch (ArgumentOutOfRangeException)
        {
            ShowDialog(ErrorMessage.DividingNotSupported);
        }
    }

    public async Task SelectFolder()
    {
        try
        {
            var file = await GetSaveFolderAsync();
            if (file is null)
            {
                ShowDialog(ErrorMessage.FolderNotPicked);
                return;
            }
            parcelBoxHelper.OutputFolder = file.Path.AbsolutePath;
        }
        catch
        {
            ShowDialog(ErrorMessage.Undefined);
        }
    }

    private static async Task<IStorageFolder?> GetSaveFolderAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
        if (provider.CanPickFolder)
        {
            var folders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { Title = "Çıktı yerini seçin" });
            return folders?.Count >= 1 ? folders[0] : null;
        }
        else
        {
            throw new PlatformNotSupportedException("Folder picking is not supported on: " + RuntimeInformation.OSDescription);
        }
    }
}