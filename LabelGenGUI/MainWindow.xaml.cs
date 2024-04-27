using ImageSharpLabelGen;
using System.Windows;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (shoeData.SelectedBrand is not null &&
                shoeData.SelectedQuality is not null &&
                shoeData.Color is not null &&
                shoeData.ReceiptNo is not null)
            {
                ShoeWriter.WriteParcelAndBox("output", shoeData.ShoeCounts, shoeData.SelectedBrand, shoeData.SelectedQuality, shoeData.Color, shoeData.ReceiptNo);
            }
            else
            {
                MessageBox.Show("Please fill all inputs properly!");
            }
        }
    }
}