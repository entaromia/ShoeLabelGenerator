using CommunityToolkit.Mvvm.Input;
using ImageSharpLabelGen.Helpers;
using LabelGenGUI.Services;
using System;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels;

public partial class MainViewModel(FilesService filesService) : ViewModelBase
{
    private static void NavigateTo(NavigationService.Pages name)
    {
        NavigationService.Instance.Navigate(name);
    }

    private bool ProjectOpen() => ShoeListService.Instance.ProjectOpen;
    private bool CanSave() => ShoeListService.Instance.ItemCount > 0;

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private async Task SaveProject()
    {
        if (ShoeListService.Instance.CurrentFile is null)
        {
            var file = await filesService.GetSaveFileAsync();
            if (file is not null)
                ShoeListService.Instance.CurrentFile = file;
        }
        await ShoeListService.Instance.SaveToFileAsync();
    }

    [RelayCommand]
    private void NewProject()
    {
        CloseProject();
        ShoeListService.Instance.ProjectOpen = true;
        if (!NavigationService.Instance.ContentHasPage)
            GoToListView();
    }

    [RelayCommand]
    private async Task OpenProject()
    {
        var file = await filesService.GetOpenFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file;

            // Fail silently if the file is unsupported
            var result = await ShoeListService.Instance.OpenFileAsync();
            if (result)
            {   
                if (!NavigationService.Instance.ContentHasPage)
                    GoToListView();
            }
        }
    }

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private void CloseProject()
    {
        NavigationService.Instance.MainPage();
        ShoeListService.Instance.CloseProject();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Print() => NavigateTo(NavigationService.Pages.PrintPage);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsPicture()
    {
        var folder = await filesService.GetFolderPathAsync();
        if (folder is not null)
        {
            LabelImageSaveHelper.SaveToPng(ShoeListService.Instance.GetItems(), folder);
        }
    }

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private void GoToListView() => NavigateTo(NavigationService.Pages.ShoeListPage);

    [RelayCommand]
    private void GoBack() => NavigationService.Instance.NavigateBack();
}