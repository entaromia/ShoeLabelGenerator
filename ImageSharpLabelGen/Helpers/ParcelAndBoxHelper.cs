using ImageSharpLabelGen.Writers;

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
        public void WriteParcelAndBox(IEnumerable<int> shoeCounts, string? brand, string? quality, string? color, string? receiptNo)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(brand) ||
               string.IsNullOrEmpty(quality) ||
               string.IsNullOrEmpty(color) ||
               string.IsNullOrEmpty(receiptNo))
            {
                throw new ArgumentNullException(nameof(brand), "The required inputs cannot be empty");
            }

            int sum = shoeCounts.Sum();

            // The maximum parcel size we have is 12
            // divide into two parcels if more than that
            // If it's more than what we support, pass it directly without dividing
            if (sum > 12 && sum < 24)
            {
                var lists = ShoeCountDivider.DivideShoeList(shoeCounts);
                foreach (var list in lists)
                {
                    parcelWriter.Write(list, brand, quality, color, receiptNo);
                    boxWriter.Write(list, brand, quality, color);
                }
            }
            else
            {
                parcelWriter.Write(shoeCounts, brand, quality, color, receiptNo);
                boxWriter.Write(shoeCounts, brand, quality, color);
            }
        }
    }
}
