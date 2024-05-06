namespace LabelGenGUI.Avalonia.ViewModels
{
    public partial class ErrorViewModel(ViewModelBase.ErrorMessage err) : ViewModelBase
    {
        public string Message { get; set; } = err switch
        {
            ErrorMessage.EmptyInputFields => "Lütfen gereken tüm alanları doldurun.",
            ErrorMessage.DividingNotSupported => "18'li, 22'li veya 24'ten büyük koliler şuan desteklenmiyor.",
            ErrorMessage.FolderNotPicked => "Lütfen bir klasör seçin.",
            _ => "Invalid error occured",
        };
    }
}
