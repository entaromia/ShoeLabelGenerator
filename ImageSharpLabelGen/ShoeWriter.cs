using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Collections.ObjectModel;

namespace ImageSharpLabelGen
{
    /// <summary>
    /// This is a common class that both parcel and box writers use
    /// </summary>
    public abstract class ShoeWriter(int imageWidth, int imageHeight) : IWriteText
    {
        public int ImageWidth { get; set; } = imageWidth;
        public int ImageHeight { get; set; } = imageHeight;
        public SolidBrush TextBrush { get; set; } = Brushes.Solid(Color.Black);
        public SolidBrush BackgroundBrush { get; set; } = Brushes.Solid(Color.White); 
        public Font BodyFont { get; set; } = SystemFonts.CreateFont("Arial", 50, FontStyle.Bold);
        public string OutputDirectory = Path.Join("output", DateTime.Now.ToString("dd-MM-yyyy-HHmmss"));

        // making sure each field has the same length so the ':' symbol always stays at the same place between lines
        public string QualityText { get; } = "KALİTE".PadRight(14, ' ');
        public string ColorText { get; } = "RENK".PadRight(15, ' ');

        /// <summary>
        /// Converts an int only shoe counts list to a "shoe number : shoe count KeyValuePair"
        /// <br/>Starts from 38 to 45
        /// </summary>
        /// <param name="countList">Int based shoe counts list</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ShoeListToKeyValuePairList(ObservableCollection<int> countList)
        {
            List<KeyValuePair<string, string>> keyValuePairs = [];

            // shoe numbers start from 38 all the way up to 45
            int currentShoeNumber = 38;
            int total = 0;
            foreach (int item in countList)
            {
                // the list that we receive from the ui has zero values in it, skip them
                if (item == 0)
                {
                    currentShoeNumber++;
                    continue;
                }

                total += item;

                var keyValue = new KeyValuePair<string, string>(currentShoeNumber.ToString(), item.ToString());
                keyValuePairs.Add(keyValue);

                currentShoeNumber++;
            }

            keyValuePairs.Add(new KeyValuePair<string, string>("TOTAL", total.ToString()));
            return keyValuePairs;
        }
    }
}
