using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageSharpLabelGen.Output
{
    public class LabelImage
    {
        public Image<L8>? Image { get; set; }
        public int Copy {  get; set; }
        public int? ShoeSize { get; set; }
    }
}
