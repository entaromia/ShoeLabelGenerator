using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LabelGenGUI.Services;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels
{
    public partial class PrintViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool progressBarVisible;

        [ObservableProperty]
        private string progressMessage = "";

        [ObservableProperty]
        private int totalBox = 0;

        [ObservableProperty]
        private int totalParcel = 0;

        public void GetTotalBox() => TotalBox = ShoeListService.Instance.GetTotalBox();
        public void GetTotalParcel() => TotalParcel = ShoeListService.Instance.GetTotalParcel();

        public async Task PrintBox()
        {
            ProgressBarVisible = true;
            ProgressMessage = "Kutu etiketi yazdırılacak...";
            await Task.Delay(100);
            var printed = await Task.Run(PrintBoxJob);
            if (printed)
            {
                ProgressMessage = "Kutular yazdırıldı.";
                ProgressBarVisible = false;
            }
        }

        public async Task PrintParcel()
        {
            ProgressBarVisible = true;
            ProgressMessage = "Koli etiketi yazdırılacak...";
            await Task.Delay(100);
            var printed = await Task.Run(PrintParcelJob);
            if (printed)
            {
                ProgressMessage = "Koliler yazdırıldı.";
                ProgressBarVisible = false;
            }
        }

        public async Task<bool> PrintBoxJob()
        {
            string zplText = PrintService.Instance.BoxesToZpl(ShoeListService.Instance.GetItems());
            UpdateUI("Etiketler oluşturuldu");
            await Task.Delay(150);
            UpdateUI("Etiketler yazdırılıyor");
            return await PrintService.Instance.PrintZpl(zplText);
        }

        public async Task<bool> PrintParcelJob()
        {
            string zplText = PrintService.Instance.ParcelsToZpl(ShoeListService.Instance.GetItems());
            UpdateUI("Etiketler oluşturuldu");
            await Task.Delay(150);
            UpdateUI("Etiketler yazdırılıyor");
            return await PrintService.Instance.PrintZpl(zplText);
        }

        private void UpdateUI(string message)
        {
            Dispatcher.UIThread.Post(() => { ProgressMessage = message; });
        }
    }
}
