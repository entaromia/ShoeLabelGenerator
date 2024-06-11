namespace LabelGenGUI.ViewModels
{
    public partial class ErrorViewModel(ViewModelBase.ErrorMessage err) : ViewModelBase
    {
        public string Message { get; set; } = err switch
        {
            ErrorMessage.EmptyInputFields => "Lütfen gereken tüm alanları doldurun.",
            ErrorMessage.FolderNotPicked => "Lütfen bir klasör seçin.",
            _ => "Invalid error occured",
        };
    }
}
