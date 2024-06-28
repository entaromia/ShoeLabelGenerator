using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageSharpLabelGen.Zpl
{
    public class ZplImage
    {
        public string? HexString { get; set; }
        public int BytesPerRow { get; set; }
        public int TotalBytes { get; set; }
    }

    /// <summary>
    /// Converts ImageSharp images pixel data to hex representation
    /// </summary>
    public static class ImageToHex
    {
        /// <summary>
        /// Takes an L8 grayscale or RGBA32 color image and converts it to ZPL compatible hex 
        /// </summary>
        /// <param name="image">The image to convert. Pixel format must be L8 or RGBA32</param>
        public static ZplImage ConvertImage<TPixel>(Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel>
        {
            bool isGrayscale = typeof(TPixel) == typeof(L8);
            bool isRgba32 = typeof(TPixel) == typeof(Rgba32);

            if (!isGrayscale && !isRgba32) throw new ArgumentException("Pixel format is not supported", nameof(image));

            // 1bpp pixel format uses 1 bit for each pixel, 1 byte for every 8 pixel
            int bytesPerRow = image.Width % 8 > 0
                ? image.Width / 8 + 1
                : image.Width / 8;

            int totalBytes = bytesPerRow * image.Height;

            // Preallocate the StringBuilder based on hex length -> 1 byte is 2 digits
            var hex = new StringBuilder(totalBytes * 2);

            image.ProcessPixelRows(accessor =>
            {
                int bits = 0;
                int currentBit = 0;

                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<TPixel> pixelRow = accessor.GetRowSpan(y);
                    int currPixel = 1;

                    // Get a reference to the pixel
                    foreach (ref TPixel pixel in pixelRow)
                    {
                        if (isGrayscale)
                        {
                            // If the pixel is black set 1 for that bit
                            L8 grayPixel = Unsafe.As<TPixel, L8>(ref pixel);

                            if (grayPixel.PackedValue < 128)
                                bits |= 1 << (7 - currentBit);
                        }
                        else
                        {
                            // First convert the color pixel to grayscale, then if the pixel is black set 1 for that bit
                            // Conversion is based on the BT.709 formula
                            Rgba32 rgbaPixel = Unsafe.As<TPixel, Rgba32>(ref pixel);

                            if ((rgbaPixel.R * .2126F) + (rgbaPixel.G * .7152F) + (rgbaPixel.B * .0722F) < 128)
                                bits |= 1 << (7 - currentBit);
                        }

                        currentBit++;

                        if (currentBit == 8 || currPixel == image.Width)
                        {
                            hex.Append(bits.ToString("X2"));
                            bits = 0;
                            currentBit = 0;
                        }

                        currPixel++;
                    }
                }
            });

            return new ZplImage
            {
                HexString = hex.ToString(),
                BytesPerRow = bytesPerRow,
                TotalBytes = totalBytes
            };
        }
    }
}
