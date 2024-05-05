using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpLabelGen
{
    /// <summary>
    /// This class writes small box labels
    /// Size: 4x6
    /// </summary>
    public class BoxWriter(string outputDir) : ShoeWriter
    {
        private readonly string boxDir = Path.Combine(outputDir, "kutu");

        private const int imageWidth = 500;
        private const int imageHeight = 400;

        // We just want a larger text for the brand name, same for everything else
        public override Font BodyFont { get; set; } = SystemFonts.CreateFont("Arial", 35, FontStyle.Bold);
        public override Font BrandFont { get; set; } = SystemFonts.CreateFont("Arial", 45, FontStyle.Bold);

        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        private readonly string qualityText = "KALİTE".PadRight(8, ' ');
        private readonly string colorText = "RENK".PadRight(9, ' ');
        private readonly string shoeNoText = "NO".PadRight(12, ' ');

        // half of the image x for centering text vertically
        private readonly PointF brandTextLocation = new(imageWidth / 2, 40);
        private readonly PointF groupTextLocation = new(imageWidth / 2, 110);

        public void WriteBox(IEnumerable<int> shoeCounts, string brand, string quality, string color)
        {
            var date = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");

            var qualityInput = PadInput(quality, 3);
            var colorInput = PadInput(color, 3);
            var shoeList = ShoeListToKeyValuePairList(shoeCounts);

            var brandText = new BrandText(BrandFont, brand) { Location = brandTextLocation };

            Directory.CreateDirectory(boxDir);

            // Generate seperate labels for every single pair
            foreach (var shoe in shoeList)
            {
                // We add a "total" key for parcel writing, skip that
                if (shoe.Key == "TOTAL")
                {
                    break;
                }
                // ---- pattern ----
                // kalite: <quality input>
                // renk: <color input>
                // no: <shoe no>
                // shoe key is the shoe number
                var group = $"{qualityText}:{qualityInput}\n{colorText}:{colorInput}\n{shoeNoText}:{PadInput(shoe.Key, 2)}";

                var groupText = new GroupText(BodyFont, group) { Location = groupTextLocation, LineSpacing = 2.4F };

                using var image = new Image<Rgba32>(imageWidth, imageHeight);
                image.Mutate(x =>
                x.Fill(BackgroundBrush)
                .DrawText(brandText.TextOptions, brandText.Text, TextBrush).
                DrawText(groupText.TextOptions, groupText.Text, TextBrush));

                // we want to save a pic for every single pair
                // if there is 3 42 shoes for example, we want to save 3 of the exact same picture
                for (int i = 0; i < Convert.ToInt32(shoe.Value); i++)
                {
                    image.SaveAsPng(Path.Combine(boxDir, $"{date}-{shoe.Key}-{i + 1}.png"));
                }
            }
        }
    }
}
