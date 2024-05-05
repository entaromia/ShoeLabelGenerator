using System;

namespace LabelGenGUI.Avalonia.ViewModels
{
    public partial class ErrorViewModel : ViewModelBase
    {
        public string Message { get; set; } = "To be replaced with actual message";
        public ErrorViewModel(Exception ex)
        {
            if (ex is ArgumentNullException)
            {
                Message = "Lütfen gereken tüm alanları doldurun.";
            }
            else if (ex is ArgumentOutOfRangeException)
            {
                Message = "18'li, 22'li veya 24'ten büyük koliler şuan desteklenmiyor.";
            }
            else
            {
                Message = ex.Message;
            }
        }
    }
}
