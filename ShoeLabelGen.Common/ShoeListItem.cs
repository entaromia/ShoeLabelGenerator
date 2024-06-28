using System.Collections.ObjectModel;

namespace ShoeLabelGen.Common
{
    public class ShoeListItem
    {
        public ShoeListItem() { }

        public ShoeListItem(ShoeListItem item) 
        { 
            Brand = item.Brand;
            Quality = item.Quality;
            Color = item.Color;
            ReceiptNo = item.ReceiptNo;
            ShoeCounts = new ObservableCollection<int>(item.ShoeCounts);
            Total = item.Total;
        }

        public string? Brand { get; set; }

        public string? Quality { get; set; }

        public string? Color { get; set; }

        public string? ReceiptNo { get; set; }

        // We support 8 different shoe pairs for now
        public ObservableCollection<int> ShoeCounts { get; set; } = [0, 0, 0, 0, 0, 0, 0, 0];

        public int Total { get; set; } = 0;
    }
}
