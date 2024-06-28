using Avalonia.Controls;
using LabelGenGUI.ViewModels;

namespace LabelGenGUI.Views
{
    public partial class PrintView : UserControl
    {
        private PrinterListViewModel printerListViewModel = new();
        public PrintView()
        {
            InitializeComponent();
            PrinterListContent.Content = printerListViewModel;
        }

        private void GetTotalCounts(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
        {
            if (DataContext is PrintViewModel model)
            {
                model.GetTotalBox();
                model.GetTotalParcel();
            }
        }
    }
}
