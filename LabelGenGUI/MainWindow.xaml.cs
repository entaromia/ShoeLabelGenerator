using System.Windows;
using ImageSharpLabelGen;

namespace LabelGenGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ShoeData shoeData = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = shoeData;

            // update total shoe count on shoe count input changes
            shoeData.ShoeCounts.CollectionChanged += (sender, e) => CalculateTotal();
        }

        private void CalculateTotal()
        {
            int total = 0;
            foreach (var shoe in shoeData.ShoeCounts)
            {
                total += shoe;
            }
            shoeData.Total = total;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (shoeData.SelectedBrand is not null && 
                shoeData.SelectedQuality is not null && 
                shoeData.Color is not null && 
                shoeData.ReceiptNo is not null) 
            {
                ParcelWriter parcelWriter = new(1140, 720);
                parcelWriter.WriteParcel(shoeData.ShoeCounts, shoeData.SelectedBrand, shoeData.SelectedQuality, shoeData.Color, shoeData.ReceiptNo);
            }
            else
            {
                MessageBox.Show("Please fill all inputs properly!");
            }
        }
    }
}