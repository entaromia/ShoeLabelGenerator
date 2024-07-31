using CommunityToolkit.Mvvm.Input;
using LabelGenGUI.Services;
using System;
using System.Collections.ObjectModel;

namespace LabelGenGUI.ViewModels
{
    public partial class PrinterListViewModel : ViewModelBase
    {
        private string newPrinterName = "";
        public string NewPrinterName
        {
            get { return newPrinterName; }
            set
            {
                SetProperty(ref newPrinterName, value);
                AddPrinterCommand.NotifyCanExecuteChanged();
            }
        }

        private string newPrinterUri = "";
        public string NewPrinterUri 
        {
            get { return newPrinterUri; }
            set
            {
                SetProperty(ref newPrinterUri, value);
                AddPrinterCommand.NotifyCanExecuteChanged();
            }
        }

        public ObservableCollection<Printer> Printers => SettingsService.Instance.Settings.Printers;

        public Printer? CurrentPrinter
        {
            get => SettingsService.Instance.Settings.CurrentPrinter;
            set { SettingsService.Instance.Settings.CurrentPrinter = value; OnPropertyChanged(); }
        }

        private bool CanAddPrinter() => !string.IsNullOrEmpty(NewPrinterName) && Uri.IsWellFormedUriString(NewPrinterUri, UriKind.Absolute);

        [RelayCommand(CanExecute = nameof(CanAddPrinter))]
        private void AddPrinter()
        {
            SettingsService.Instance.Settings.Printers.Add(new Printer { Name = NewPrinterName, Uri = NewPrinterUri });
        }

        [RelayCommand]
        public void RemovePrinter()
        {
            if (CurrentPrinter is not null)
            {
                SettingsService.Instance.Settings.Printers.Remove(CurrentPrinter);
            }
        }
    }
}
