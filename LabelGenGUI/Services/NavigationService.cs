using Avalonia.Controls;
using LabelGenGUI.ViewModels;
using System.Diagnostics;

namespace LabelGenGUI.Services
{
    public class NavigationService
    {
        public static int GetViewModelFromPageName(Pages page)
        {
            return page switch
            {
                Pages.ShoeListPage => 0,
                Pages.InputPage => 1,
                Pages.PrintPage => 2,
                _ => throw new System.NotImplementedException()
            };
        }

        public enum Pages
        {
            ShoeListPage,
            InputPage,
            PrintPage
        }

        public ViewModelBase[] viewModels = [
            new ShoeListViewModel(),
            new InputViewModel(),
            new PrintViewModel()
            ];

        public static NavigationService Instance { get; } = new();

        public ContentControl? ContentControl { private get; set; }
        public bool ContentHasPage => ContentControl!.Content is not null;

        public void Navigate(ViewModelBase? viewModel)
        {
            ContentControl!.Content = viewModel;
        }

        public void Navigate(Pages page)
        {
            Debug.WriteLine($"Navigating to: {page}");
            Navigate(viewModels[GetViewModelFromPageName(page)]);
        }

        public void MainPage() => Navigate(null);
    }
}
