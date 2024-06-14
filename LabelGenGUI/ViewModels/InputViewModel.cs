using LabelGenGUI.Services;
using ShoeLabelGen.Common;

namespace LabelGenGUI.ViewModels
{
    public class InputViewModel : ViewModelBase
    {
        public int Total
        {
            get { return ShoeListItem.Total; }
            set
            {
                ShoeListItem.Total = value;
                PropertyChangedEvent();
            }
        }

        public ShoeListItem ShoeListItem { get; set; } = new();

        // User selection of the brand
        public static string[] Brands => [
            "RIDGE",
            "GERONIMO"
        ];

        // User selection of different qualities
        public static string[] Qualities => [
            "SRC 01",
            "SRC 02",
            "LFR 01",
            "LFR 02"
        ];

        public InputViewModel()
        {
            // Update total shoe count on shoe count input changes
            ShoeListItem.ShoeCounts.CollectionChanged += (_, _) =>
            {
                int total = 0;
                foreach (var count in ShoeListItem.ShoeCounts)
                {
                    total += count;
                }
                Total = total;
            };
        }

        public void AddNewItem()
        {
            // Validate inputs
            if (ShoeListItem.Brand is null ||
               ShoeListItem.Quality is null ||
               string.IsNullOrEmpty(ShoeListItem.Color) ||
               string.IsNullOrEmpty(ShoeListItem.ReceiptNo))
            {
                ShowDialog(ErrorMessage.EmptyInputFields);
            }
            else
            {
                ShoeListService.Instance.AddItem(ShoeListItem);
                GoBack();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Avalonia throws when static methods are used as commands")]
        public void GoBack()
        {
            NavigationService.Instance.Navigate(NavigationService.Pages.ShoeListPage);
        }
    }
}
