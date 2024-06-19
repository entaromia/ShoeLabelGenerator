using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen.Helpers
{
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

    public static class CommonBrushes
    {
        public static SolidBrush Text { get; set; } = Brushes.Solid(Color.Black);
        public static SolidBrush Background { get; set; } = Brushes.Solid(Color.White);
    }

    /// <summary>
    /// This is a common helper class that both parcel and box writers use
    /// </summary>
    public static class ShoeWriterHelper
    {
        /// <summary>
        /// Converts an int only shoe counts list to a "shoe number : shoe count KeyValuePair"
        /// <br/>Starts from 38 to 45
        /// </summary>
        /// <param name="countList">Int based shoe counts list</param>
        public static List<KeyValuePair<string, int>> ShoeListToKeyValuePairList(IEnumerable<int> countList)
        {
            List<KeyValuePair<string, int>> keyValuePairs = [];

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

                var keyValue = new KeyValuePair<string, int>(currentShoeNumber.ToString(), item);
                keyValuePairs.Add(keyValue);

                currentShoeNumber++;
            }
            
            return keyValuePairs;
        }

        /// <summary>
        /// Adds padding from start of the input
        /// </summary>
        public static string PadInput(this string input, int chars) => input.PadLeft(input.Length + chars, ' ');
    }
}
