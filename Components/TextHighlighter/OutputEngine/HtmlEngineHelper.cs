using System;
using System.Drawing;
using System.Text;
using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.OutputEngine
{
    [Sauerova]
    internal static class HtmlEngineHelper
    {
        /// <summary>
        /// Style wrapper around creating a pattern style string.
        /// </summary>
        /// <param name="style">Style to be used</param>
        public static string CreatePatternStyle(Style style)
        {
            return CreatePatternStyle(style.Colors, style.Font);
        }

        /// <summary>
        /// Creates a string with color and font for the style attribute.
        /// </summary>
        /// <param name="colors">Colors to be used</param>
        /// <param name="font">Font to be used</param>
        /// <returns></returns>
        public static string CreatePatternStyle(ColorPair colors, Font font)
        {
            var patternStyle = new StringBuilder();
            if (colors != null) {
                if (colors.ForeColor != Color.Empty) {
                    patternStyle.Append("color: " + colors.ForeColor.Name + ";");
                }
                if (colors.BackColor != Color.Empty) {
                    patternStyle.Append("background-color: " + colors.BackColor.Name + ";");
                }
            }

            if (font != null) {
                if (font.Name != null) {
                    patternStyle.Append("font-family: " + font.Name + ";");
                }
                if (font.Size > 0f) {
                    patternStyle.Append("font-size: " + font.Size + "px;");
                }
                if (font.Style == FontStyle.Regular) {
                    patternStyle.Append("font-weight: normal;");
                }
                if (font.Style == FontStyle.Bold) {
                    patternStyle.Append("font-weight: bold;");
                }
            }

            return patternStyle.ToString();
        }
    }
}