using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using LabelGenGUI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LabelGenGUI.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void PropertyChangedEvent([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SetProperty<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(property, value))
        {
            return;
        }
        property = value;
        PropertyChangedEvent(propertyName);
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