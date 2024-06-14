using ImageSharpLabelGen.Writers;
using ShoeLabelGen.Common;

namespace ImageSharpLabelGen.Helpers
{
    public class ParcelAndBoxHelper
    {
        private readonly ParcelWriter parcelWriter = new();
        private readonly BoxWriter boxWriter = new();

        public string? OutputFolder
        {
            get => parcelWriter.OutputFolder;
            set
            {
                parcelWriter.OutputFolder = value;
                boxWriter.OutputFolder = value;
            }
        }

        /// <summary>
        /// Creates both parcel and box labels
        /// </summary>
        public void WriteParcelAndBox(ShoeListItem item)
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
                    parcelWriter.Write(list);
                    boxWriter.Write(list);
                }
            }
            else
            {
                parcelWriter.Write(item);
                boxWriter.Write(item);
            }
        }

        public void WriteParcelAndBox(IEnumerable<ShoeListItem> items)
        {
            foreach (var item in items)
            {
                WriteParcelAndBox(item);
            }
        }
    }
}
