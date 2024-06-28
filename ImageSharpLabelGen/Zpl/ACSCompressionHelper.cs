using System.Text;

namespace ImageSharpLabelGen.Zpl
{
    /// <summary>
    /// Alternative Data Compression Scheme (ACS) for ~DG, ~DB and ~GF Commands
    /// <br/> This is an alternative data compression scheme recognized by the Zebra printers. 
    /// <br/> Further reduces the amount of time required to download graphic images and
    /// bitmapped fonts
    /// </summary>
    public static class ACSCompressionHelper
    {
        /// <summary>
        /// (CompressionCountMapping -> g)
        /// </summary>
        const int MinCompressionBlockCount = 20;

        /// <summary>
        /// (CompressionCountMapping -> z)
        /// </summary>
        const int MaxCompressionRepeatCount = 400;

        /// <summary>
        /// The mapping table used for compression.
        /// Each character count (the key) is represented by a certain char (the value).
        /// </summary>
        private static readonly Dictionary<int, char> CompressionCountMapping = new()
        {
            {1, 'G'}, {2, 'H'}, {3, 'I'}, {4, 'J'}, {5, 'K'}, {6, 'L'}, {7, 'M'}, {8, 'N'}, {9, 'O' }, {10, 'P'},
            {11, 'Q'}, {12, 'R'}, {13, 'S'}, {14, 'T'}, {15, 'U'}, {16, 'V'}, {17, 'W'}, {18, 'X'}, {19, 'Y'},
            {20, 'g'}, {40, 'h'}, {60, 'i'}, {80, 'j' }, {100, 'k'}, {120, 'l'}, {140, 'm'}, {160, 'n'}, {180, 'o'}, {200, 'p'},
            {220, 'q'}, {240, 'r'}, {260, 's'}, {280, 't'}, {300, 'u'}, {320, 'v'}, {340, 'w'}, {360, 'x'}, {380, 'y'}, {400, 'z' }
        };

        /// <summary>
        /// Compress hex using Alternative Compression Scheme
        /// </summary>
        /// <param name="hexData">Clean hex data 000000\nFFFFFF</param>
        /// <returns>The hex string compressed using ACS</returns>
        public static string Compress(string hexData, int bytesPerRow)
        {
            // one byte is two chars in hex
            var chunkSize = bytesPerRow * 2;

            var cleanHexData = hexData.Replace("\n", string.Empty).Replace("\r", string.Empty);

            var compressedLines = new StringBuilder(cleanHexData.Length);
            var compressedCurrentLine = new StringBuilder(chunkSize);
            var compressedPreviousLine = string.Empty;

            var span = cleanHexData.AsSpan();

            int currentIndex = 0;

            for (int i = 0; i < cleanHexData.Length / chunkSize; i++)
            {
                var currentLine = span.Slice(currentIndex, chunkSize);
                currentIndex += chunkSize;
                var lastChar = currentLine[0];
                var charRepeatCount = 1;

                for (var j = 1; j < currentLine.Length; j++)
                {
                    if (currentLine[j] == lastChar)
                    {
                        charRepeatCount++;
                        continue;
                    }

                    //char changed within the line
                    compressedCurrentLine.Append(GetZebraCharCount(charRepeatCount));
                    compressedCurrentLine.Append(lastChar);

                    lastChar = currentLine[j];
                    charRepeatCount = 1;
                }

                //process last char in current line
                if (lastChar.Equals('0'))
                {
                    //fills the line, to the right, with zeros
                    compressedCurrentLine.Append(',');
                }
                else if (lastChar.Equals('1'))
                {
                    //fills the line, to the right, with ones
                    compressedCurrentLine.Append('!');
                }
                else
                {
                    compressedCurrentLine.Append(GetZebraCharCount(charRepeatCount));
                    compressedCurrentLine.Append(lastChar);
                }

                if (compressedCurrentLine.ToString().Equals(compressedPreviousLine))
                {
                    //previous line is repeated
                    compressedLines.Append(':');
                }
                else
                {
                    compressedLines.Append(compressedCurrentLine);
                }

                compressedPreviousLine = compressedCurrentLine.ToString();
                compressedCurrentLine.Clear();
            }
            return compressedLines.ToString();
        }

        public static string GetZebraCharCount(int charRepeatCount)
        {
            //only append if the repeated character is than 1 otherwise the compression scheme uses 2 characters to represent 1
            if (charRepeatCount == 1)
            {
                return string.Empty;
            }
            if (CompressionCountMapping.TryGetValue(charRepeatCount, out var compressionKey))
            {
                return $"{compressionKey}";
            }

            var compressionKeys = new StringBuilder(5);

            // Get repeat count in multiples of 20 
            int multi20 = charRepeatCount / MinCompressionBlockCount * MinCompressionBlockCount;

            // Remaining chars to repeat
            var remainder = charRepeatCount % multi20;

            while (remainder > 0 || multi20 > 0)
            {
                if (multi20 > MaxCompressionRepeatCount)
                {
                    remainder += multi20 - MaxCompressionRepeatCount;
                    multi20 = MaxCompressionRepeatCount;
                }

                if (!CompressionCountMapping.TryGetValue(multi20, out compressionKey))
                {
                    throw new Exception("Compression failure");
                }

                compressionKeys.Append(compressionKey);

                if (remainder == 0)
                {
                    break;
                }

                if (remainder <= 20)
                {
                    CompressionCountMapping.TryGetValue(remainder, out compressionKey);
                    compressionKeys.Append(compressionKey);
                    break;
                }

                multi20 = remainder / MinCompressionBlockCount * MinCompressionBlockCount;
                remainder %= multi20;
            }

            return compressionKeys.ToString();
        }
    }
}
