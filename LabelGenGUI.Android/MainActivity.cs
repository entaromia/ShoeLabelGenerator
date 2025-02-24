using Android.App;
using Android.Content.PM;

using Avalonia;
using Avalonia.Android;
using LabelGenGUI.Services;
using System.Diagnostics;

namespace LabelGenGUI.Android;

[Activity(
    Label = "LabelGenGUI.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    protected override void OnStop()
    {
        Debug.WriteLine("Application is closing, save printer settings!");
        SettingsService.Instance.Save();
        base.OnStop();
    }
}
