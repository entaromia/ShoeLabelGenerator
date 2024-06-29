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
            File.WriteAllText("test.txt", data);
            await Task.Delay(100);
            return true;
#else
            var response = await client.PrintJobAsync(request);

            var jobStatusRequest = new GetJobAttributesRequest
            {
                JobId = response.JobId,
                PrinterUri = printerUri,
                RequestedAttributes = [JobAttribute.JobState]
            };

            var jobStatusResponse = await client.GetJobAttributesAsync(jobStatusRequest);

            while (jobStatusResponse.JobAttributes.JobState != JobState.Completed)
            {
                await Task.Delay(250);
                jobStatusResponse = await client.GetJobAttributesAsync(jobStatusRequest);
            }

            return true;
#endif
        }
    }
}
