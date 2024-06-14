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
    public class ParcelWriter : ShoeWriter
    {
        private const int imageWidth = 1198; // 15cm in 203dpi
        private const int imageHeight = 799; // 10cm in 203dpi

        // We just want a larger text for the brand name, same for everything else
        public override Font BodyFont { get; set; } = SystemFonts.CreateFont("Arial", 50, FontStyle.Bold);
        public override Font BrandFont { get; set; } = SystemFonts.CreateFont("Arial", 85, FontStyle.Bold);

        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        private readonly string qualityText = "KALİTE".PadRight(14, ' ');
        private readonly string colorText = "RENK".PadRight(15, ' ');
        private readonly string shoeNoText = "FİŞ NO".PadRight(15, ' ');

        // half of the image x for centering text vertically
        private readonly PointF brandTextLocation = new(imageWidth / 2, 60);
        private readonly PointF groupTextLocation = new(imageWidth / 2, 220);

        public async Task Write(ShoeListItem item)
        {
            ArgumentNullException.ThrowIfNull(OutputFolder);
            string parcelDir = Path.Combine(OutputFolder, "koli");

            var date = DateTime.Now.Ticks;

            var qualityInput = PadInput(item.Quality!, 5);
            var colorInput = PadInput(item.Color!, 5);
            var receiptNoInput = PadInput(item.ReceiptNo!, 5);

            // ---- pattern ----
            // kalite: <quality input>
            // renk: <color input>
            // fis no: <receipt no input>
            var group = $"{qualityText}:{qualityInput}\n{colorText}:{colorInput}\n{shoeNoText}:{receiptNoInput}";

            var brandText = new BrandText(BrandFont, item.Brand!) { Location = brandTextLocation };

            var groupText = new GroupText(BodyFont, group) { Location = groupTextLocation };

            var shoeCountsPair = ShoeListToKeyValuePairList(item.ShoeCounts);

            var shoeCountTextOptions = new RichTextOptions(BodyFont)
            {
                LineSpacing = 1.2F,
                TextAlignment = TextAlignment.Center
            };

            Directory.CreateDirectory(parcelDir);

            using var image = new Image<L8>(imageWidth, imageHeight);
            image.Mutate(x =>
                x.Fill(BackgroundBrush)
                .DrawText(brandText.TextOptions, brandText.Text, TextBrush)
                .DrawText(groupText.TextOptions, groupText.Text, TextBrush)
                .WritePairs(shoeCountsPair, shoeCountTextOptions, TextBrush));

            await image.SaveAsPngAsync(Path.Combine(parcelDir, $"{item.Brand!}-{date}.png"));
        }
    }
}
