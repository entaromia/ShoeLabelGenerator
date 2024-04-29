using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LabelGenGUI.Avalonia.ViewModels;

public class ViewModelBase: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void PropertyChangedEvent([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
