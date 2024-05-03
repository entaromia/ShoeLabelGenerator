using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen
{
    static class TextCalculator
    {
        /// <summary>
        /// Joins key and value strings in a KeyValuePair with a new line between them
        /// </summary>
        public static string JoinKeyValuePair(KeyValuePair<string, string> pair) => $"{pair.Key}\n{pair.Value}";

        /// <summary>
        /// Calculates total text width of multiple string,string KeyValuePair 
        /// </summary>
        public static float TotalPairTextLength(List<KeyValuePair<string, string>> keyValuePairs, RichTextOptions textOptions, float padding = 0)
        {
            var width = 0F;
            foreach (var pair in keyValuePairs)
            {
                var text = JoinKeyValuePair(pair);
                float textLength = TextMeasurer.MeasureBounds(text, textOptions).Width;

                // don't add a padding if this is the first string we are iterating
                width += width == 0 ? textLength : textLength + padding;
            }
            return width;
        }
    }
}
