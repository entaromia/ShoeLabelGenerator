using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;

namespace ImageSharpLabelGen
{
    /// <summary>
    /// This class writes the big parcel labels
    /// Size: 10x15
    /// </summary>
    public class ParcelWriter(int imageWidth, int imageHeight) : ShoeWriter(imageWidth, imageHeight)
    {
        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        public string ReceiptNoText { get; set; } = "FİŞ NO".PadRight(15, ' ');

        // We just want a larger text for the brand name, same for everything else
        public readonly Font BrandFont = SystemFonts.CreateFont("Arial", 85, FontStyle.Bold);

        // half of the image x for centering text vertically
        public readonly PointF brandTextLocation = new(imageWidth / 2, 40);
        public readonly PointF groupTextLocation = new(imageWidth / 2, 200);

        public void WriteParcel(ObservableCollection<int> shoeCounts, string brand, string quality, string color, string receiptNo)
        {
            // make them 5 chars apart from the ':' symbol that seperates the field name and value
            // example : "kalite  :  src 01"
            var qualityInput = quality.PadLeft(quality.Length + 5, ' ');
            var colorInput = color.PadLeft(color.Length + 5, ' ');
            var receiptNoInput = receiptNo.PadLeft(receiptNo.Length + 5, ' ');

            // ---- pattern ----
            // kalite: <quality input>
            // renk: <color input>
            // fis no: <receipt no input>
            var group = $"{QualityText}:{qualityInput}\n{ColorText}:{colorInput}\n{ReceiptNoText}:{receiptNoInput}";

            var brandText = new ImageSharpText(BrandFont)
            {
                Location = brandTextLocation,
                Text = brand,
                TextDecorations = TextDecorations.Underline,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            var groupText = new ImageSharpText(BodyFont)
            {
                Location = groupTextLocation,
                Text = group,
                HorizontalAlignment = HorizontalAlignment.Center,
                LineSpacing = 1.8F
            };

            var shoeCountsPair = ShoeListToKeyValuePairList(shoeCounts);
            var shoeCountTextOptions = new RichTextOptions(BodyFont)
            {
                LineSpacing = 1.2F,
                TextAlignment = TextAlignment.Center
            };

            Directory.CreateDirectory(OutputDirectory);

            using var image = new Image<Rgba32>(ImageWidth, ImageHeight);
            image.Mutate(x =>
            x.Fill(BackgroundBrush)
            .DrawText(brandText.TextOptions, brandText.Text, TextBrush)
            .DrawText(groupText.TextOptions, groupText.Text, TextBrush)
            .WritePairs(shoeCountsPair, shoeCountTextOptions, TextBrush));

            image.SaveAsPng(Path.Combine(OutputDirectory, "koli.png"));
        }
    }
}
