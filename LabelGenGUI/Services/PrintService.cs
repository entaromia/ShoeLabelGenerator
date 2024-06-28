using ImageSharpLabelGen.Output;
using ImageSharpLabelGen.Printers;
using ShoeLabelGen.Common;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelGenGUI.Services
{
    /// <summary>
    /// This service provides printing functionality for ZPL-supported printers
    /// </summary>
    public class PrintService
    {
        public static PrintService Instance { get; } = new();
        public IPPPrinter Printer { get; set; } = new();

        public string ParcelsToZpl(IEnumerable<ShoeListItem> items)
        {
            StringBuilder zplString = new();
            foreach (var item in items)
            {
                zplString.Append(item.ParcelToZpl());
            }
            return zplString.ToString();
        }

        public string BoxesToZpl(IEnumerable<ShoeListItem> items)
        {
            StringBuilder zplString = new();
            foreach (var item in items)
            {
                item.BoxToZpl();
            }
            return zplString.ToString();
        }

        public async Task<bool> PrintZpl(string data)
        {
            Printer.PrinterUrl = "http://192.168.1.1:631/printers/XprinterXP-470B";//SettingsService.Instance.Settings.CurrentPrinter!.Uri;
            return await Printer.Print(data);
        }
    }
}
