using Avalonia.Controls.ApplicationLifetimes;
using ImageSharpLabelGen;
using LabelGenGUI.Avalonia.Views;
using System;
using System.Collections.ObjectModel;

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
        parcelBoxHelper = new ParcelAndBoxHelper("output");

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
        if (SelectedBrand is not null &&
               SelectedQuality is not null &&
               Color is not null &&
               ReceiptNo is not null)
        {
            parcelBoxHelper.WriteParcelAndBox(ShoeCounts, SelectedBrand, SelectedQuality, Color, ReceiptNo);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(App.Current);
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ArgumentNullException.ThrowIfNull(desktop.MainWindow);
                new ErrorWindow().ShowDialog(desktop.MainWindow);
            }
        }
    }
}
