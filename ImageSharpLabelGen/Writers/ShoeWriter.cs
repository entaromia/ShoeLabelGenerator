using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen.Writers
{
    /// <summary>
    /// This is a common class that both parcel and box writers use
    /// </summary>
    public abstract class ShoeWriter : IWriteText
    {
        public string? OutputFolder { get; set; }
        public SolidBrush TextBrush { get; set; } = Brushes.Solid(Color.Black);
        public SolidBrush BackgroundBrush { get; set; } = Brushes.Solid(Color.White);

        public abstract Font BrandFont { get; set; }
        public abstract Font BodyFont { get; set; }

        public class BrandText : ImageSharpText
        {
            public BrandText(Font font, string text) : base(font, text)
            {
                HorizontalAlignment = HorizontalAlignment.Center;
                TextDecorations = TextDecorations.Underline;
            }
        }

        public class GroupText : ImageSharpText
        {
            public GroupText(Font font, string text) : base(font, text)
            {
                HorizontalAlignment = HorizontalAlignment.Center;
                LineSpacing = 2F;
            }
        }

        /// <summary>
        /// Converts an int only shoe counts list to a "shoe number : shoe count KeyValuePair"
        /// <br/>Starts from 38 to 45
        /// </summary>
        /// <param name="countList">Int based shoe counts list</param>
        public static List<KeyValuePair<string, string>> ShoeListToKeyValuePairList(IEnumerable<int> countList)
        {
            List<KeyValuePair<string, string>> keyValuePairs = [];

            // shoe numbers start from 38 all the way up to 45
            int currentShoeNumber = 38;
            int total = 0;
            foreach (int item in countList)
            {
                // if the current shoe number has 0 count, don't write it
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

            // total is written on parcel labels
            keyValuePairs.Add(new KeyValuePair<string, string>("TOTAL", total.ToString()));
            return keyValuePairs;
        }

        /// <summary>
        /// Adds padding from start of the input
        /// </summary>
        public static string PadInput(string input, int chars) => input.PadLeft(input.Length + chars, ' ');
    }
}
