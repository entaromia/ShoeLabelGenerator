using ImageSharpLabelGen.Writers;
using ShoeLabelGen.Common;
using SixLabors.ImageSharp;

namespace ImageSharpLabelGen.Output
{
    public static class PictureSaver
    {
        public static void SaveAsPicture(this ShoeListItem item, string outputFolder)
        {
            ArgumentNullException.ThrowIfNull(item.Brand);
            string parcelDir = Path.Combine(outputFolder, "koli");
            string boxDir = Path.Combine(outputFolder, "kutu");

            var date = DateTime.Now.Ticks;

            Directory.CreateDirectory(parcelDir);
            Directory.CreateDirectory(boxDir);

            var parcelLabel = ParcelWriter.Write(item);
            parcelLabel.Image.SaveAsPngAsync(Path.Combine(parcelDir, $"{item.Brand}-{date}.png"));
            parcelLabel.Image?.Dispose();

            var boxLabel = BoxWriter.Write(item);
            foreach (var label in boxLabel)
            {
                for (int i = 0; i < Convert.ToInt32(label.Copy); i++)
                {
                    label.Image.SaveAsPngAsync(Path.Combine(boxDir, $"{item.Brand}-{date}-{label.ShoeSize}-{i + 1}.png"));
                }
                label.Image?.Dispose();
            }
        }
    }
}
