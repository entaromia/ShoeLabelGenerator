using System.Buffers.Binary;
using System.Text;

namespace MiniSharpIpp
{
    public class IppPrint
    {
        private readonly HttpClient httpClient = new();
        public async Task<bool> PrintAsync(PrintJobRequest request, CancellationToken cancellationToken = default)
        {
            using MemoryStream stream = new();
            WriteIppRequest(stream, request);
            stream.Seek(0, SeekOrigin.Begin);

            // File.WriteAllBytes("test.bin", stream.ToArray());

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, request.PrinterUri)
            {
                Content = new StreamContent(stream) { Headers = { { "Content-Type", "application/ipp" } } }
            };

            var response = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private void WriteIppRequest(Stream stream, PrintJobRequest request)
        {
            ArgumentNullException.ThrowIfNull(stream);
            var writer = new BinaryWriter(stream);
            
            writer.Write(BinaryPrimitives.ReverseEndianness((short)0x0101)); // ipp version 1.1
            writer.Write(BinaryPrimitives.ReverseEndianness((short)0x0002)); // print job operation
            writer.Write(BinaryPrimitives.ReverseEndianness(1)); // request 1
            writer.Write((byte)0x01); // operation group tag
            WriteAttribute(writer, 0x47, "attributes-charset", "utf-8");
            WriteAttribute(writer, 0x48, "attributes-natural-language", "en");
            WriteAttribute(writer, 0x45, "printer-uri", request.PrinterUri.ToString());

            // add document format if specified
            if (request.DocumentFormat is not null)
            {
                WriteAttribute(writer, 0x49, "document-format", request.DocumentFormat);
            }

            writer.Write((byte)0x03); // end tag
            request.Document.CopyTo(stream); // append document

        }

        public void WriteAttribute(BinaryWriter writer, int tag, string name, string value)
        {
            writer.Write((byte)tag);
            writer.Write(BinaryPrimitives.ReverseEndianness((short)name.Length));
            writer.Write(Encoding.UTF8.GetBytes(name));
            writer.Write(BinaryPrimitives.ReverseEndianness((short)value.Length));
            writer.Write(Encoding.UTF8.GetBytes(value));
        }
    }
}
