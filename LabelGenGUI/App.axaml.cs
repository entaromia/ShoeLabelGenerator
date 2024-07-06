using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using LabelGenGUI.Services;
using LabelGenGUI.ViewModels;
using LabelGenGUI.Views;
using System;

namespace LabelGenGUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.Exit += DesktopOnExit;
            var window = new MainWindow();
            window.DataContext = new MainViewModel(new FilesService(window));
            desktop.MainWindow = window;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var view = new MainView();
            view.DataContext = new MainViewModel(new FilesService(TopLevel.GetTopLevel(view) ?? throw new NullReferenceException("Missing TopLevel on MainView")));
            singleViewPlatform.MainView = new MainView();
        }

        SettingsService.Instance.Load();

        base.OnFrameworkInitializationCompleted();
    }

    private void DesktopOnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        SettingsService.Instance.Save();
    }
}
