using LabelGenGUI.Services;
using ShoeLabelGen.Common;
using System.Collections.ObjectModel;

namespace LabelGenGUI.ViewModels
{
    public class ShoeListViewModel() : ViewModelBase
    {
        public static ObservableCollection<ShoeListItem> ListItems => ShoeListService.Instance.GetItems();

        /// <summary>
        /// Do not allow deleting if no item is selected
        /// </summary>
        public bool CanDeleteItem(object msg) => (int)msg >= 0;
        public void DeleteItem(object msg)
        {
            ShoeListService.Instance.DeleteItem((int)msg);
        }

        public void AddItem()
        {
            NavigationService.Instance.Navigate(NavigationService.ViewModelName.InputViewModel);
        }
    }
}
