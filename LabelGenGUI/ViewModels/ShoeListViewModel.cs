using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelGenGUI.Services;
using ShoeLabelGen.Common;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels
{
    public partial class ShoeListViewModel() : ViewModelBase
    {
        public static ObservableCollection<ShoeListItem> ListItems => ShoeListService.Instance.GetItems();

        [ObservableProperty]
        private int selectedIndex;

        /// <summary>
        /// Do not allow operations if no item is selected
        /// </summary>
        public bool CanOperateOnItem() => SelectedIndex >= 0;

        [RelayCommand(CanExecute = nameof(CanOperateOnItem))]
        public void DeleteItem()
        {
            ShoeListService.Instance.DeleteItem(SelectedIndex);
        }

        public void AddItem()
        {
            NavigationService.Instance.Navigate(NavigationService.Pages.InputPage);
        }

        [RelayCommand(CanExecute = nameof(CanOperateOnItem))]
        public async Task PrintBoxItem()
        {
            string zplText = PrintService.Instance.BoxesToZpl([ListItems[SelectedIndex]]);
            await PrintService.Instance.PrintZpl(zplText);
        }

        [RelayCommand(CanExecute = nameof(CanOperateOnItem))]
        public async Task PrintParcelItem(object msg)
        {
            string zplText = PrintService.Instance.ParcelsToZpl([ListItems[SelectedIndex]]);
            await PrintService.Instance.PrintZpl(zplText);
        }
    }
}
