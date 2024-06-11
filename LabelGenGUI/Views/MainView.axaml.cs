using Avalonia;
using Avalonia.Controls;
using LabelGenGUI.Services;

namespace LabelGenGUI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        NavigationService.Instance.ContentControl = MainContentControl;
    }
}
