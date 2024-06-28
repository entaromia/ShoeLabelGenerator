using SharpIpp.Models;
using SharpIpp.Protocol.Models;
using SharpIpp;
using System.Text;

namespace ImageSharpLabelGen.Printers
{
    public class IPPPrinter : IPrintLabel
    {
        public string? PrinterUrl { get; set; }
        private readonly DocumentAttributes rawDocumentAttribute = new() { DocumentFormat = "application/vnd.cups-raw" };
        private readonly SharpIppClient client = new();

        public async Task<bool> Print(string data)
        {
            if (PrinterUrl == null) throw new NullReferenceException("The printer url was null!");

            // The SharpIpp apis accept stream only, convert text to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            using MemoryStream stream = new(byteArray);
            var printerUri = new Uri(PrinterUrl);
            var request = new PrintJobRequest
            {
                PrinterUri = printerUri,
                Document = stream,
                DocumentAttributes = rawDocumentAttribute
            };
#if false
            await Task.Delay(1000);
            return true;
#else
            var response = await client.PrintJobAsync(request);

            if (response.JobState == JobState.Completed)
            {
                return true;
            }
            return false;
#endif
        }
    }
}
