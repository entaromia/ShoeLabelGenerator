﻿using CommunityToolkit.Mvvm.Input;
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

    private bool ProjectOpen() => ShoeListService.Instance.CurrentFile is not null;
    private bool CanSave() => ShoeListService.Instance.ItemCount > 0;

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private async Task SaveProject()
    {
        if (ShoeListService.Instance.CurrentFile is null)
        {
            throw new NullReferenceException("Missing current file path");
        }
        await ShoeListService.Instance.SaveToFileAsync();
    }

    [RelayCommand]
    private async Task NewProject()
    {
        if (OperatingSystem.IsAndroid())
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = "test";
            ShoeListService.Instance.ProjectName = "test";
        }
        else
        {
            var file = await filesService.GetSaveFileAsync();
            if (file is not null)
            {
                CloseProject();
                ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
                ShoeListService.Instance.ProjectName = file.Name.Contains(".json") ? file.Name[..file.Name.IndexOf(".json")] : file.Name;
            }
        }

        if (!NavigationService.Instance.ContentHasPage)
                GoToListView();
    }

    [RelayCommand]
    private async Task OpenProject()
    {
        if (OperatingSystem.IsAndroid()) return;
        var file = await filesService.GetOpenFileAsync();
        if (file is not null)
        {
            CloseProject();
            ShoeListService.Instance.CurrentFile = file.Path.AbsolutePath;
            await ShoeListService.Instance.OpenFileAsync();
            if (!NavigationService.Instance.ContentHasPage)
                GoToListView();
        }
    }

    [RelayCommand(CanExecute = nameof(ProjectOpen))]
    private void CloseProject()
    {
        NavigationService.Instance.MainPage();
        ShoeListService.Instance.CloseProject();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Print()
    {
        NavigateTo(NavigationService.Pages.PrintPage);
    }

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