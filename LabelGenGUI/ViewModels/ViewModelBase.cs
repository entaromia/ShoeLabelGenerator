using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using LabelGenGUI.Views;
using System;

namespace LabelGenGUI.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected static void ShowDialog(ErrorMessage err)
    {
        if (Application.Current is not { } app || app.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow is not { } window)
            throw new NullReferenceException();
        new ErrorWindow()
        {
            DataContext = new ErrorViewModel(err)
        }.ShowDialog(window);
    }

    public enum ErrorMessage
    {
        EmptyInputFields,
        FolderNotPicked,
        Undefined
    }
}