namespace MiniSharpIpp
{
    public class PrintJobRequest(Uri printerUri, Stream document)
    {
        public Uri PrinterUri { get; set; } = printerUri;
        public Stream Document { get; set; } = document;
        public string? DocumentFormat { get; set; }
    }
}
