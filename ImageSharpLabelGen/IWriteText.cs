using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen
{
    interface IWriteText
    {
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        SolidBrush TextBrush { get; set; }
        SolidBrush BackgroundBrush { get; set; }
        Font BodyFont { get; set; }
    }
}
