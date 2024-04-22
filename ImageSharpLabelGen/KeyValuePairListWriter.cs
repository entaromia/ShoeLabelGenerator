using SixLabors.Fonts;
using SixLabors.Fonts.Unicode;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpLabelGen
{
    static class KeyValuePairListWriter
    {
        /// <summary>
        /// ImageSharp Extension that draws a KeyValuePair list horizontally stacked
        /// </summary>
        public static IImageProcessingContext WritePairs(this IImageProcessingContext context, List<KeyValuePair<string, string>> pairList, RichTextOptions textOptions, Brush brush)
        {
            int imageWidth = context.GetCurrentSize().Width;

            // previous pair written, converted to formatted text
            string? previousText = null;

            // amount of px padding between pairs
            // reduce padding if there is more than 8 pairs (inc total)
            int textPadding = pairList.Count > 8 ? 50 : 60;

            // total text width of the all pairs
            float totalWidth = TextCalculator.TotalPairTextLength(pairList, textOptions, textPadding);

            // the current X to start drawing text
            // centering text math: (total page width - total text width) / 2
            // this gives the x to start writing, which results with same amount of pixels at both sides of text
            float currentX = (imageWidth - totalWidth) / 2;

            foreach (var shoePair in pairList)
            {
                var currentText = TextCalculator.JoinKeyValuePair(shoePair);

                IReadOnlyList<RichTextRun> textRuns =
                [
                    // we only want key of the pair to be underlined
                    new RichTextRun()
                    {
                        Start = 0,
                        End = shoePair.Key.GetGraphemeCount(),
                        TextDecorations = TextDecorations.Underline,
                    }
                ];
                textOptions.TextRuns = textRuns;

                // if we wrote another pair before this, write next to it with some space between
                if (previousText is null)
                {
                    previousText = currentText;
                }
                else
                {
                    int previousIndex = pairList.IndexOf(shoePair) - 1;
                    previousText = TextCalculator.JoinKeyValuePair(pairList[previousIndex]);
                    currentX += TextMeasurer.MeasureBounds(previousText, textOptions).Width + textPadding;
                }

                textOptions.Origin = new PointF(currentX, 520);
                context.DrawText(textOptions, currentText, brush);
            }
            return context;
        }
    }
}
