using MiniSharpIpp;
using System.Text;

namespace ImageSharpLabelGen.Printers
{
    public class IPPPrinter : IPrintLabel
    {
        public string? PrinterUrl { get; set; }
        private readonly IppPrint printer = new();

        public async Task<bool> Print(string data)
        {
            if (PrinterUrl == null) throw new NullReferenceException("The printer url was null!");

            // The SharpIpp apis accept stream only, convert text to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            using MemoryStream stream = new(byteArray);
            var printerUri = new Uri(PrinterUrl);
            var request = new PrintJobRequest(printerUri, stream)
            {
                DocumentFormat = "application/vnd.cups-raw"
            };

            return await printer.PrintAsync(request);

            /* var jobStatusRequest = new GetJobAttributesRequest
            {
                JobId = response.JobId,
                PrinterUri = printerUri,
                RequestedAttributes = [JobAttribute.JobState]
            };

            var jobStatusResponse = await client.GetJobAttributesAsync(jobStatusRequest);

            while (jobStatusResponse.JobAttributes.JobState != JobState.Completed)
            {
                await Task.Delay(300);
                jobStatusResponse = await client.GetJobAttributesAsync(jobStatusRequest);
            } */
        }
    }
}
