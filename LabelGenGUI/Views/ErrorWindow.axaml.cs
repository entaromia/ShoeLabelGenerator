using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LabelGenGUI.Views
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }

        public void CloseButton_Click(object sender, RoutedEventArgs args)
        => Close();
    }
}
