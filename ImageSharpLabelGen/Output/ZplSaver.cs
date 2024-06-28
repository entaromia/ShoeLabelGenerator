using ImageSharpLabelGen.Helpers;
using ImageSharpLabelGen.Writers;
using ImageSharpLabelGen.Zpl;
using ShoeLabelGen.Common;
using SixLabors.ImageSharp.Processing;
using System.Text;

namespace ImageSharpLabelGen.Output
{
    public enum LabelType
    {
        Box,
        Parcel
    }
    public static class ZplSaver
    {
        public static string SaveAsRawZpl(this LabelImage image, LabelType type)
        {
            ArgumentNullException.ThrowIfNull(image.Image, nameof(image.Image));

            if (type == LabelType.Parcel)
            {
                image.Image.Mutate(x => { x.Rotate(RotateMode.Rotate90); });
            }

            var hexImage = ImageToHex.ConvertImage(image.Image);
            var compressedZpl = ACSCompressionHelper.Compress(hexImage.HexString!, hexImage.BytesPerRow);

            StringBuilder zpl2Code = new(
                "^XA" // Start the document
                + $"^PW{image.Image.Width}" // Adjust label size based on image width
                + "^FO0,0^GFA," // Graphic field initialization
                + hexImage.TotalBytes + "," + hexImage.TotalBytes + "," + hexImage.BytesPerRow + ",");

            // Compress image data using 'Alternative Data Compression Scheme'

            // Add image data
            zpl2Code.Append(compressedZpl);
            zpl2Code.Append("^FS");

            // Add repeat count if we have multiple copies of that label
            if (image.Copy > 1)
            {
                zpl2Code.Append("^PQ" + image.Copy);
            }

            // End ZPL II code
            zpl2Code.Append("^XZ");

            return zpl2Code.ToString();
        }

        public static string BoxToZpl(this ShoeListItem item)
        {
            var labels = BoxWriter.Write(item);
            StringBuilder zplStrings = new();

            foreach (var label in labels)
            {
                zplStrings.Append(label.SaveAsRawZpl(LabelType.Box));
            }
            return zplStrings.ToString();
        }

        public static string ParcelToZpl(this ShoeListItem item)
        {
            StringBuilder zplStrings = new();
            if (item.Total > 12 && item.Total < 24)
            {
                var lists = ShoeCountDivider.DivideShoeList(item);
                int i = 0;
                foreach (var list in lists)
                {
                    var label = ParcelWriter.Write(list);
                    zplStrings.Append(label.SaveAsRawZpl(LabelType.Parcel));
                    i++;
                }
            }
            else
            {
                var label = ParcelWriter.Write(item);
                zplStrings.Append(label.SaveAsRawZpl(LabelType.Parcel));
            }

            return zplStrings.ToString();
        }
    }
}
