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

    protected static void ShowDialog(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(App.Current);

        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ArgumentNullException.ThrowIfNull(desktop.MainWindow);
            new ErrorWindow()
            {
                DataContext = new ErrorViewModel(ex)
            }.ShowDialog(desktop.MainWindow);
        }
    }
}
