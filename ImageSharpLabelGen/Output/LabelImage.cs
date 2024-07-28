using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageSharpLabelGen.Output
{
    public class LabelImage(Image<L8> image) : IDisposable
    {
        public Image<L8> Image { get; set; } = image;
        public int Copy { get; set; }
        public int? ShoeSize { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Image?.Dispose();
            }
        }
    }
}