﻿using System.Drawing;

namespace Components.Models.HighlightingPatterns
{
    [Sauerova]
    public class ColorPair
    {
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public ColorPair()
        {
        }

        public ColorPair(Color foreColor, Color backColor)
        {
            ForeColor = foreColor;
            BackColor = backColor;
        }
    }
}