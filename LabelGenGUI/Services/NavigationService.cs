using Avalonia.Controls;
using LabelGenGUI.ViewModels;
using System.Diagnostics;

namespace LabelGenGUI.Services
{
    public class NavigationService
    {
        public static int GetViewModelIndex(ViewModelName name)
        {
            return name switch
            {
                ViewModelName.ShoeListViewModel => 0,
                ViewModelName.InputViewModel => 1,
                _ => throw new System.NotImplementedException()
            };
        }

        public enum ViewModelName
        {
            ShoeListViewModel,
            InputViewModel
        }

        public ViewModelBase[] viewModels = [
            new ShoeListViewModel(),
            new InputViewModel()
            ];

        public static NavigationService Instance { get; } = new();

        public ContentControl? ContentControl { private get; set; }
        public bool ContentHasPage => ContentControl!.Content is not null;

        public void Navigate(ViewModelBase? viewModel)
        {
            ContentControl!.Content = viewModel;
        }

        public void Navigate(ViewModelName name)
        {
            Debug.WriteLine($"Navigating to: {name}");
            Navigate(viewModels[GetViewModelIndex(name)]);
        }

        public void MainPage() => Navigate(null);
    }
}
