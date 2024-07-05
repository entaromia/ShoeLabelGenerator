using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using LabelGenGUI.Services;
using System;
using System.Collections.ObjectModel;

namespace LabelGenGUI.ViewModels
{
    public partial class PrinterListViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string newPrinterName = "";

        [ObservableProperty]
        private string newPrinterUri = "";

        public ObservableCollection<Printer> Printers => SettingsService.Instance.Settings.Printers;

        public Printer? CurrentPrinter
        {
            get => SettingsService.Instance.Settings.CurrentPrinter;
            set { SettingsService.Instance.Settings.CurrentPrinter = value; OnPropertyChanged(); }
        }

        [DependsOn(nameof(NewPrinterName))]
        [DependsOn(nameof(NewPrinterUri))]
        public bool CanAddPrinter(object msg) => !string.IsNullOrEmpty(NewPrinterName) && Uri.IsWellFormedUriString(NewPrinterUri, UriKind.Absolute);

        public void AddPrinter()
        {
            SettingsService.Instance.Settings.Printers.Add(new Printer { Name = NewPrinterName, Uri = NewPrinterUri });
        }

        public bool CanRemovePrinter(object msg) => msg is not null;
        public void RemovePrinter()
        {
            if (CurrentPrinter is not null)
            {
                SettingsService.Instance.Settings.Printers.Remove(CurrentPrinter);

            }
        }
    }
}
