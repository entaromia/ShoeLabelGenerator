using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using LabelGenGUI.Avalonia.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LabelGenGUI.Avalonia.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void PropertyChangedEvent([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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