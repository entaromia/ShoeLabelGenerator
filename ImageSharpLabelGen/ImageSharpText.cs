﻿using SixLabors.Fonts;
using SixLabors.Fonts.Unicode;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageSharpLabelGen
{
    /// <summary>
    /// Wraps a string text and RichTextOptions together
    /// <br/> Allows setting and accessing some text properties easier
    /// </summary>
    /// <param name="font">The font to use</param>
    public class ImageSharpText(Font font, string text = "")
    {
        public string Text { get; set; } = text;

        public RichTextOptions TextOptions { get; private set; } = new RichTextOptions(font);

        public IReadOnlyList<RichTextRun> TextRuns { get; private set; } = [];

        public PointF Location
        {
            get => TextOptions.Origin;
            set => TextOptions.Origin = value;
        }

        public TextDecorations TextDecorations
        {
            set
            {
                TextRuns = [
                    new RichTextRun()
                    {
                        Start = 0,
                        End = Text.GetGraphemeCount(),
                        TextDecorations = TextDecorations.Underline
                    }
                ];
                TextOptions.TextRuns = TextRuns;
            }
        }

        public TextAlignment TextAlignment
        {
            get => TextOptions.TextAlignment;
            set => TextOptions.TextAlignment = value;
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get => TextOptions.HorizontalAlignment;
            set => TextOptions.HorizontalAlignment = value;
        }

        public float LineSpacing
        {
            get => TextOptions.LineSpacing;
            set => TextOptions.LineSpacing = value;
        }
    }
}
