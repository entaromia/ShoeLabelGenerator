using CommunityToolkit.Mvvm.Input;
using LabelGenGUI.Services;
using ShoeLabelGen.Common;
using System;

namespace LabelGenGUI.ViewModels
{
    public partial class InputViewModel : ViewModelBase
    {
        public int Total
        {
            get { return ShoeListItem.Total; }
            set
            {
                ShoeListItem.Total = value;
                OnPropertyChanged();
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

        [RelayCommand]
        public void AddNewItem()
        {
            // Validate inputs
            if (ShoeListItem.Brand is null ||
               ShoeListItem.Quality is null ||
               string.IsNullOrEmpty(ShoeListItem.Color) ||
               string.IsNullOrEmpty(ShoeListItem.ReceiptNo))
            {
                // FIXME: Replace it with something else for Android compat
                if (!OperatingSystem.IsAndroid())
                    ShowDialog(ErrorMessage.EmptyInputFields);
                return;
            }

            ShoeListService.Instance.AddItem(new ShoeListItem(ShoeListItem));
            NavigationService.Instance.NavigateBack();
        }
    }
}
