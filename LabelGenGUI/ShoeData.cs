using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LabelGenGUI
{
    /*
     * ViewModel of MainWindow
     */
    public class ShoeData : INotifyPropertyChanged
    {
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


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void PropertyChangedEvent([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
