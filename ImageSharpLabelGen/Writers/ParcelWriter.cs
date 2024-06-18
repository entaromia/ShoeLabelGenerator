using ImageSharpLabelGen.Helpers;
using ImageSharpLabelGen.Output;
using ShoeLabelGen.Common;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpLabelGen.Writers
{
    /// <summary>
    /// This class writes the big parcel labels
    /// Size: 10x15
    /// </summary>
    public static class ParcelWriter
    {
        private const int imageWidth = 1198; // 15cm in 203dpi
        private const int imageHeight = 799; // 10cm in 203dpi

        // We just want a larger text for the brand name, same for everything else
        public static Font BodyFont { get; set; } = SystemFonts.CreateFont("Arial", 50, FontStyle.Bold);
        public static Font BrandFont { get; set; } = SystemFonts.CreateFont("Arial", 85, FontStyle.Bold);

        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        private static readonly string qualityText = "KALİTE".PadRight(14, ' ');
        private static readonly string colorText = "RENK".PadRight(15, ' ');
        private static readonly string shoeNoText = "FİŞ NO".PadRight(15, ' ');

        // half of the image x for centering text vertically
        private static readonly PointF brandTextLocation = new(imageWidth / 2, 60);
        private static readonly PointF groupTextLocation = new(imageWidth / 2, 220);

        public static LabelImage Write(ShoeListItem item)
        {
            var qualityInput = item.Quality!.PadInput(5);
            var colorInput = item.Color!.PadInput(5);
            var receiptNoInput = item.ReceiptNo!.PadInput(5);

            // ---- pattern ----
            // kalite: <quality input>
            // renk: <color input>
            // fis no: <receipt no input>
            var group = $"{qualityText}:{qualityInput}\n{colorText}:{colorInput}\n{shoeNoText}:{receiptNoInput}";

            var brandText = new BrandText(BrandFont, item.Brand!) { Location = brandTextLocation };

            var groupText = new GroupText(BodyFont, group) { Location = groupTextLocation };

            var shoeCountsPair = ShoeWriterHelper.ShoeListToKeyValuePairList(item.ShoeCounts);

            var shoeCountTextOptions = new RichTextOptions(BodyFont)
            {
                LineSpacing = 1.2F,
                TextAlignment = TextAlignment.Center
            };

            var image = new Image<L8>(imageWidth, imageHeight);
            image.Mutate(x =>
                x.Fill(CommonBrushes.Background)
                .DrawText(brandText.TextOptions, brandText.Text, CommonBrushes.Text)
                .DrawText(groupText.TextOptions, groupText.Text, CommonBrushes.Text)
                .WritePairs(shoeCountsPair, shoeCountTextOptions, CommonBrushes.Text));

            return new LabelImage() { Image = image , Copy = 1};
        }
    }
}
