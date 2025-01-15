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
        // Reset printer page configuration
        public static string ResetLabelFormatZpl => "^XA^PON^LH0,0^FWN^XZ";

        public static string SaveAsRawZpl(this LabelImage image, LabelType type)
        {
            if (type == LabelType.Parcel)
            {
                image.Image.Mutate(x => { x.Rotate(RotateMode.Rotate90); });
            }

            var hexImage = ImageToHex.ConvertImage(image.Image);

            // Compress image data using 'Alternative Data Compression Scheme'
            var compressedZpl = ACSCompressionHelper.Compress(hexImage.HexString!, hexImage.BytesPerRow);

            StringBuilder zpl2Code = new(
                "^XA" // Start the document
                + $"^PW{image.Image.Width}" // Adjust label size based on image width
                + "^FO0,0^GFA," // Graphic field initialization
                + hexImage.TotalBytes + "," + hexImage.TotalBytes + "," + hexImage.BytesPerRow + ",");

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
            StringBuilder zplStrings = new(ResetLabelFormatZpl);
            var labels = BoxWriter.Write(item);
            
            foreach (var label in labels) using (label)
                {
                    zplStrings.Append(label.SaveAsRawZpl(LabelType.Box));
                }
            return zplStrings.ToString();
        }

        public static string ParcelToZpl(this ShoeListItem item)
        {
            StringBuilder zplStrings = new(ResetLabelFormatZpl);
            // The maximum parcel size we have is 12
            // divide into two parcels if more than that
            // If it's more than 2 * 12, pass it directly without dividing
            if (item.Total > 12 && item.Total <= 24)
            {
                var lists = ShoeCountDivider.DivideShoeList(item);
                int i = 0;
                foreach (var list in lists)
                {
                    using (var label = ParcelWriter.Write(list))
                        zplStrings.Append(label.SaveAsRawZpl(LabelType.Parcel));
                    i++;
                }
            }
            else
            {
                using var label = ParcelWriter.Write(item);
                zplStrings.Append(label.SaveAsRawZpl(LabelType.Parcel));
            }

            return zplStrings.ToString();
        }
    }
}
