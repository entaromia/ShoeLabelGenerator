namespace ImageSharpLabelGen
{
    public class ParcelAndBoxHelper(string outDir)
    {
        private readonly ParcelWriter parcelWriter = new(outDir);
        private readonly BoxWriter boxWriter = new(outDir);

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
            if (sum == 18 || sum == 22 || sum > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(shoeCounts), "Dividing these inputs are not supported yet");
            }

            // The maximum parcel size we have is 12
            // divide into two parcels if more than that
            if (sum > 12)
            {
                var lists = ShoeCountDivider.DivideShoeList(shoeCounts);
                foreach (var list in lists)
                {
                    parcelWriter.WriteParcel(list, brand, quality, color, receiptNo);
                    boxWriter.WriteBox(list, brand, quality, color);
                }
            }
            else
            {
                parcelWriter.WriteParcel(shoeCounts, brand, quality, color, receiptNo);
                boxWriter.WriteBox(shoeCounts, brand, quality, color);
            }
        }
    }
}
