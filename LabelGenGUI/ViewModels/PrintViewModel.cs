using Avalonia.Threading;
using LabelGenGUI.Services;
using System.Threading.Tasks;

namespace LabelGenGUI.ViewModels
{
    public class PrintViewModel : ViewModelBase
    {
        private bool progressVisible;
        public bool ProgressVisible { get => progressVisible; set => SetProperty(ref progressVisible, value); }

        private bool progressBarVisible;
        public bool ProgressBarVisible { get => progressBarVisible; set => SetProperty(ref progressBarVisible, value); }

        private string progressMessage = "hi";
        public string ProgressMessage { get => progressMessage; set => SetProperty(ref progressMessage, value); }

        private int totalBox = 0;
        public int TotalBox { get => totalBox; set => SetProperty(ref totalBox, value); }

        private int totalParcel = 0;
        public int TotalParcel { get => totalParcel; set => SetProperty(ref totalParcel, value); }

        public void GetTotalBox() => TotalBox = ShoeListService.Instance.GetTotalBox();
        public void GetTotalParcel() => TotalParcel = ShoeListService.Instance.GetTotalParcel();

        public async Task PrintBox()
        {
            ProgressVisible = true;
            ProgressBarVisible = true;
            ProgressMessage = "Kutu etiketi yazdırılacak...";
            Task.Delay(100).Wait();
            var printed = await Task.Run(PrintBoxJob);
            if (printed)
            {
                ProgressMessage = "Kutular yazdırıldı.";
                ProgressBarVisible = false;
            }
        }

        public async Task PrintParcel()
        {
            ProgressVisible = true;
            ProgressBarVisible = true;
            ProgressMessage = "Koli etiketi yazdırılacak...";
            Task.Delay(100).Wait();
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
            Task.Delay(150).Wait();
            UpdateUI("Etiketler yazdırılıyor");
            return await PrintService.Instance.PrintZpl(zplText);
        }

        public async Task<bool> PrintParcelJob()
        {
            string zplText = PrintService.Instance.ParcelsToZpl(ShoeListService.Instance.GetItems());
            UpdateUI("Etiketler oluşturuldu");
            Task.Delay(150).Wait();
            UpdateUI("Etiketler yazdırılıyor");
            return await PrintService.Instance.PrintZpl(zplText);
        }

        private void UpdateUI(string message)
        {
            Dispatcher.UIThread.Post(() => { ProgressMessage = message; });
        }

        public void GoBack()
        {
            NavigationService.Instance.Navigate(NavigationService.Pages.ShoeListPage);
        }
    }
}
