using System.Collections.ObjectModel;

namespace ImageSharpLabelGen
{
    public class ParcelAndBoxHelper(string outDir)
    {
        private readonly ParcelWriter parcelWriter = new(outDir);
        private readonly BoxWriter boxWriter = new(outDir);

        /// <summary>
        /// Creates both parcel and box labels
        /// </summary>
        public void WriteParcelAndBox(ObservableCollection<int> shoeCounts, string brand, string quality, string color, string receiptNo)
        {
            parcelWriter.WriteParcel(shoeCounts, brand, quality, color, receiptNo);
            boxWriter.WriteBox(shoeCounts, brand, quality, color);
        }
    }
}
