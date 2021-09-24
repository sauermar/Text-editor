using System.Drawing;

namespace Components.Models.HighlightingPatterns
{
    public class Style
    {
        [Sauerova]
        public ColorPair Colors { get; private set; }
        public Font Font { get; private set; }

        public Style(ColorPair colors, Font font)
        {
            Colors = colors;
            Font = font;
        }
    }
}