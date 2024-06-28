using Avalonia.Metadata;
using LabelGenGUI.Services;
using System;
using System.Collections.Generic;

namespace LabelGenGUI.ViewModels
{
    public class PrinterListViewModel : ViewModelBase
    {
        private string newPrinterName = "";
        public string NewPrinterName { get => newPrinterName; set => SetProperty(ref newPrinterName, value); }

        private string newPrinterUri = "";
        public string NewPrinterUri { get => newPrinterUri; set => SetProperty(ref newPrinterUri, value); }

        private string selectedPrinterName = "";
        public string SelectedPrinterName => SettingsService.Instance.Settings.CurrentPrinter?.Name ?? "";

        public List<Printer> Printers => SettingsService.Instance.Settings.Printers;

        [DependsOn(nameof(NewPrinterName))]
        [DependsOn(nameof(NewPrinterUri))]
        public bool CanAddPrinter(object msg) => !string.IsNullOrEmpty(NewPrinterName) && Uri.IsWellFormedUriString(NewPrinterUri, UriKind.Absolute);

        public void AddPrinter()
        {
            SettingsService.Instance.Settings.Printers.Add(new Printer { Name = NewPrinterName, Uri = NewPrinterUri });
        }
    }
}
