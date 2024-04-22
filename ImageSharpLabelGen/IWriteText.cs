using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen
{
    interface IWriteText
    {
        SolidBrush TextBrush { get; set; }
        SolidBrush BackgroundBrush { get; set; }
        Font BodyFont { get; set; }
    }
}
