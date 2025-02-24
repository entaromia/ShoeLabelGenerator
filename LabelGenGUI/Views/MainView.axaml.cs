using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using LabelGenGUI.Services;
using System;
using System.Diagnostics;

namespace LabelGenGUI.Views;

public partial class MainView : UserControl
{
    private TopLevel? topLevel;
    private IInputPane? inputPane;

    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        NavigationService.Instance.ContentControl = MainContentControl;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        // Soft keyboard workaround for Android until Avalonia does it automatically
        if (OperatingSystem.IsAndroid())
        {
            if (TopLevel.GetTopLevel(this) is TopLevel topLevel && topLevel.InputPane is IInputPane inputPane)
            {
                this.topLevel = topLevel;
                this.inputPane = inputPane;
                inputPane.StateChanged += InputPane_StateChanged;
            }
        }
    }

    // TODO: Do something when keyboard changes its height while still being in same state
    // Like when switching from text keyboard to number keyboard
    private void InputPane_StateChanged(object? sender, InputPaneStateEventArgs e)
    {
        if (e.NewState != InputPaneState.Open)
            return;
        
        if (topLevel is null || inputPane is null)
            return;

        var keyboardHeight = inputPane.OccludedRect.Height;
        var newHeight = topLevel.Height - keyboardHeight;

        Debug.WriteLine($"Setting current height from {topLevel.Height} to : {newHeight}");

        topLevel.Height = newHeight;
    }
}
