using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Components.Extensions;
using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.Configuration
{
    [Sauerova]
    /// <summary>
    /// Reads configuration for highliting from XML file.
    /// </summary>
    public class XmlConfiguration : IConfiguration
    {
        private IDictionary<string, Definition> definitions;
        public IDictionary<string, Definition> Definitions
        {
            get { return GetDefinitions(); }
        }

        public XDocument XmlDocument { get; protected set; }

        public XmlConfiguration(XDocument xmlDocument)
        {
            XmlDocument = xmlDocument ?? throw new ArgumentNullException("xmlDocument");
        }

        protected XmlConfiguration()
        {
        }

        /// <summary>
        /// Gets individual definitions from the passed XML document 
        /// </summary>
        private IDictionary<string, Definition> GetDefinitions()
        {
            if (definitions == null) {
                definitions = XmlDocument
                    .Descendants("definition")
                    .Select(GetDefinition)
                    .ToDictionary(x => x.Name);
            }

            return definitions;
        }

        /// <summary>
        /// Parses a single definition from XML document.
        /// </summary>
        /// <param name="definitionElement">A single definition to be parsed</param>
        /// <returns></returns>
        private Definition GetDefinition(XElement definitionElement)
        {
            var name = definitionElement.GetAttributeValue("name");
            var patterns = GetPatterns(definitionElement);
            var caseSensitive = Boolean.Parse(definitionElement.GetAttributeValue("caseSensitive"));
            var style = GetDefinitionStyle(definitionElement);

            return new Definition(name, caseSensitive, style, patterns);
        }

        /// <summary>
        /// Gets all patterns which follows the definition element.
        /// </summary>
        /// <param name="definitionElement">A single definition to be parsed</param>
        /// <returns></returns>
        private IDictionary<string, Pattern> GetPatterns(XContainer definitionElement)
        {
            var patterns = definitionElement
                .Descendants("pattern")
                .Select(GetPattern)
                .ToDictionary(x => x.Name);

            return patterns;
        }

        /// <summary>
        /// Gets a pattern type from the pattern element.
        /// </summary>
        /// <param name="patternElement">The pattern element to be parsed</param>
        /// <returns></returns>
        private Pattern GetPattern(XElement patternElement)
        {
            const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
            var patternType = patternElement.GetAttributeValue("type");
            if (patternType.Equals("block", stringComparison)) {
                return GetBlockPattern(patternElement);
            }
            if (patternType.Equals("markup", stringComparison)) {
                return GetMarkupPattern(patternElement);
            }
            if (patternType.Equals("word", stringComparison)) {
                return GetWordPattern(patternElement);
            }

            throw new InvalidOperationException(String.Format("Unknown pattern type: {0}", patternType));
        }

        /// <summary>
        /// Gets the pattern type block element's attributes.
        /// </summary>
        /// <param name="patternElement">The pattern element of type block, which attributes will be parsed</param>
        /// <returns></returns>
        private BlockPattern GetBlockPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            var beginsWith = patternElement.GetAttributeValue("beginsWith");
            var endsWith = patternElement.GetAttributeValue("endsWith");
            var escapesWith = patternElement.GetAttributeValue("escapesWith");

            return new BlockPattern(name, style, beginsWith, endsWith, escapesWith);
        }

        /// <summary>
        /// Gets the pattern type markup element's attributes.
        /// </summary>
        /// <param name="patternElement">The pattern element of type markup, which attributes will be parsed</param>
        /// <returns></returns>
        private MarkupPattern GetMarkupPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            var highlightAttributes = Boolean.Parse(patternElement.GetAttributeValue("highlightAttributes"));
            var bracketColors = GetMarkupPatternBracketColors(patternElement);
            var attributeNameColors = GetMarkupPatternAttributeNameColors(patternElement);
            var attributeValueColors = GetMarkupPatternAttributeValueColors(patternElement);

            return new MarkupPattern(name, style, highlightAttributes, bracketColors, attributeNameColors, attributeValueColors);
        }

        /// <summary>
        /// Gets the pattern type word element's attributes.
        /// </summary>
        /// <param name="patternElement">The pattern element of type word, which attributes will be parsed</param>
        /// <returns></returns>
        private WordPattern GetWordPattern(XElement patternElement)
        {
            var name = patternElement.GetAttributeValue("name");
            var style = GetPatternStyle(patternElement);
            var words = GetPatternWords(patternElement);

            return new WordPattern(name, style, words);
        }

        /// <summary>
        /// Gets all the words that follows a pattern element.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetPatternWords(XContainer patternElement)
        {
            var words = new List<string>();
            var wordElements = patternElement.Descendants("word");
            if (wordElements != null) {
                words.AddRange(from wordElement in wordElements select Regex.Escape(wordElement.Value));
            }

            return words;
        }

        /// <summary>
        /// Gets all attributes of style from font elements that follows pattern element.
        /// </summary>
        /// <returns></returns>
        private Style GetPatternStyle(XContainer patternElement)
        {
            var fontElement = patternElement.Descendants("font").Single();
            var colors = GetPatternColors(fontElement);
            var font = GetPatternFont(fontElement);

            return new Style(colors, font);
        }

        /// <summary>
        /// Returns all color pairs from the font element.
        /// </summary>
        private ColorPair GetPatternColors(XElement fontElement)
        {
            var foreColor = Color.FromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = Color.FromName(fontElement.GetAttributeValue("backColor"));

            return new ColorPair(foreColor, backColor);
        }

        /// <summary>
        /// Returns a font that is defined on the font element or defaultFont if passed as an argument.
        /// </summary>
        private Font GetPatternFont(XElement fontElement, Font defaultFont = null)
        {
            var fontFamily = fontElement.GetAttributeValue("name");
            if (fontFamily != null) {
                var emSize = fontElement.GetAttributeValue("size").ToSingle(11f);
                var style = Enum<FontStyle>.Parse(fontElement.GetAttributeValue("style"), FontStyle.Regular, true);

                return new Font(fontFamily, emSize, style);
            }

            return defaultFont;
        }

        /// <summary>
        /// Gets the brackets colors from pattern element of type markup.
        /// </summary>
        private ColorPair GetMarkupPatternBracketColors(XContainer patternElement)
        {
            const string descendantName = "bracketStyle";
            return GetMarkupPatternColors(patternElement, descendantName);
        }

        /// <summary>
        /// Gets the attribute name colors from pattern element of type markup.
        /// </summary>
        private ColorPair GetMarkupPatternAttributeNameColors(XContainer patternElement)
        {
            const string descendantName = "attributeNameStyle";
            return GetMarkupPatternColors(patternElement, descendantName);
        }

        /// <summary>
        /// Gets the attribute values colors from pattern element of type markup.
        /// </summary>
        private ColorPair GetMarkupPatternAttributeValueColors(XContainer patternElement)
        {
            const string descendantName = "attributeValueStyle";
            return GetMarkupPatternColors(patternElement, descendantName);
        }

        /// <summary>
        /// Returns colors of elements following the pattern element with descendant name.
        /// </summary>
        /// <param name="patternElement"></param>
        /// <param name="descendantName">Name of the following elements</param>
        /// <returns></returns>
        private ColorPair GetMarkupPatternColors(XContainer patternElement, XName descendantName)
        {
            var fontElement = patternElement.Descendants("font").Single();
            var element = fontElement.Descendants(descendantName).SingleOrDefault();
            if (element != null) {
                var colors = GetPatternColors(element);

                return colors;
            }

            return null;
        }

        /// <summary>
        /// Gets style of the definition element. The style consists of colors and font.
        /// </summary>
        private Style GetDefinitionStyle(XNode definitionElement)
        {
            const string xpath = "default/font";
            var fontElement = definitionElement.XPathSelectElement(xpath);
            var colors = GetDefinitionColors(fontElement);
            var font = GetDefinitionFont(fontElement);

            return new Style(colors, font);
        }

        /// <summary>
        /// Gets all the colors from font element of definition element.
        /// </summary>
        private ColorPair GetDefinitionColors(XElement fontElement)
        {
            var foreColor = Color.FromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = Color.FromName(fontElement.GetAttributeValue("backColor"));

            return new ColorPair(foreColor, backColor);
        }

        /// <summary>
        /// Gets the font value from font element of definition element.
        /// </summary>
        private Font GetDefinitionFont(XElement fontElement)
        {
            var fontName = fontElement.GetAttributeValue("name");
            var fontSize = Convert.ToSingle(fontElement.GetAttributeValue("size"));
            var fontStyle = (FontStyle) Enum.Parse(typeof(FontStyle), fontElement.GetAttributeValue("style"), true);

            return new Font(fontName, fontSize, fontStyle);
        }
    }
}