using ImageSharpLabelGen.Output;
using ImageSharpLabelGen.Writers;
using ShoeLabelGen.Common;

namespace ImageSharpLabelGen.Helpers
{
    /// <summary>
    /// This extension class saves lists box and parcel labels to picture
    /// </summary>
    public static class LabelImageSaveHelper
    {
        public static void SaveToPng(ShoeListItem item, string outputFolder)
        {
            // Validate inputs
            if (item.Brand is null ||
               item.Quality is null ||
               string.IsNullOrEmpty(item.Color) ||
               string.IsNullOrEmpty(item.ReceiptNo))
            {
                throw new ArgumentNullException(nameof(item), "Required inputs cannot be empty");
            }

            // The maximum parcel size we have is 12
            // divide into two parcels if more than that
            // If it's more than what we support, pass it directly without dividing
            if (item.Total > 12 && item.Total < 24)
            {
                var lists = ShoeCountDivider.DivideShoeList(item);
                foreach (var list in lists)
                {
                    list.SaveAsPicture(outputFolder);
                }
            }
            else
            {
                item.SaveAsPicture(outputFolder);
            }
        }

        public static void SaveToPng(IEnumerable<ShoeListItem> items, string outputFolder)
        {
            foreach (var item in items)
            {
                SaveToPng(item, outputFolder);
            }
        }
    }
}
