using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LabelGenGUI.Services;
using LabelGenGUI.ViewModels;
using LabelGenGUI.Views;

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
            desktop.Exit += DesktopOnExit;
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        SettingsService.Instance.Load();

        base.OnFrameworkInitializationCompleted();
    }

    private void DesktopOnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        SettingsService.Instance.Save();
    }
}
