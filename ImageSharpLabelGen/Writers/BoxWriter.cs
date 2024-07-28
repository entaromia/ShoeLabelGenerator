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
    /// This class writes small box labels
    /// Size: 4x6
    /// </summary>
    public static class BoxWriter
    {
        private const int imageWidth = 479; // 6cm in 203dpi
        private const int imageHeight = 319; // 4cm in 203dpi

        // We just want a larger text for the brand name, same for everything else
        public static Font BodyFont { get; set; } = SystemFonts.CreateFont("Arial", 35, FontStyle.Bold);
        public static Font BrandFont { get; set; } = SystemFonts.CreateFont("Arial", 45, FontStyle.Bold);

        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        private static readonly string qualityText = "KALİTE".PadRight(8, ' ');
        private static readonly string colorText = "RENK".PadRight(9, ' ');
        private static readonly string shoeNoText = "NO".PadRight(12, ' ');

        // half of the image x for centering text vertically
        private static readonly PointF brandTextLocation = new(imageWidth / 2, 30);
        private static readonly PointF groupTextLocation = new(imageWidth / 2, 95);

        public static List<LabelImage> Write(ShoeListItem item)
        {
            List<LabelImage> imageResults = [];

            var qualityInput = item.Quality!.PadInput(3);
            var colorInput = item.Color!.PadInput(3);
            var shoeList = ShoeWriterHelper.ShoeListToKeyValuePairList(item.ShoeCounts);

            var brandText = new BrandText(BrandFont, item.Brand!) { Location = brandTextLocation };

            // As use all the available label area, use smaller font size for long colors
            if (item.Color!.Length >= 12)
            {
                // Use even smaller font for longer color names
                // Helps scaling color input like 'HAKİ FLOTTER' or 'KOYU GRİ SÜET' properly
                BodyFont = new Font(BodyFont, item.Color!.Length >= 13 ? 32 : 33);
            }

            // Generate seperate labels for every single pair
            foreach (var shoe in shoeList)
            {
                // ---- pattern ----
                // kalite: <quality input>
                // renk: <color input>
                // no: <shoe no>
                // shoe key is the shoe number
                var group = $"{qualityText}:{qualityInput}\n{colorText}:{colorInput}\n{shoeNoText}:{shoe.Key.PadInput(2)}";

                var groupText = new GroupText(BodyFont, group) { Location = groupTextLocation, LineSpacing = 2F };

                var image = new Image<L8>(imageWidth, imageHeight);
                image.Mutate(x =>
                x.Fill(CommonBrushes.Background)
                .DrawText(brandText.TextOptions, brandText.Text, CommonBrushes.Text).
                DrawText(groupText.TextOptions, groupText.Text, CommonBrushes.Text));

                // we want to save a pic for every single pair
                // if there is 3 42 shoes for example, we want to save 3 of the exact same picture
                imageResults.Add(new LabelImage(image) {Copy = Convert.ToInt32(shoe.Value), ShoeSize = Convert.ToInt32(shoe.Key) });
            }

            return imageResults;
        }
    }
}
