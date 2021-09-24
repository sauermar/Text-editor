using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.OutputEngine
{
    [Sauerova]
    public class HtmlEngine : Engine
    {
        private const string StyleSpanFormat = "<span style=\"{0}\">{1}</span>";

        /// <summary>
        /// Converts an input string to the HTML-encoded string.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="input">The string to be highlighted and converted</param>
        /// <returns></returns>
        protected override string PreHighlight(Definition definition, string input)
        {
            if (definition == null) {
                throw new ArgumentNullException("definition");
            }

            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// Adds the style attribute obtained from configuration definition to the expected HTML output.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="input">The string to be connected with the style attribute</param>
        /// <returns></returns>
        protected override string PostHighlight(Definition definition, string input)
        {
            if (definition == null) {
                throw new ArgumentNullException("definition");
            }
            var cssStyle = HtmlEngineHelper.CreatePatternStyle(definition.Style);

            return String.Format(StyleSpanFormat, cssStyle, input);
        }

        /// <summary>
        /// Processes a block type pattern element and returns the HTML output.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="pattern">Pattern to be processed</param>
        /// <param name="match">Result from regular expression to be highligted</param>
        protected override string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
        {
            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

            return String.Format(StyleSpanFormat, patternStyle, match.Value);
        }

        /// <summary>
        /// Processes a markup type pattern element and returns the HTML output.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="pattern">Pattern to be processed</param>
        /// <param name="match">Result from regular expression to be highligted</param>
        protected override string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            if (definition == null) {
                throw new ArgumentNullException("definition");
            }
            if (pattern == null) {
                throw new ArgumentNullException("pattern");
            }
            if (match == null) {
                throw new ArgumentNullException("match");
            }

            var result = new StringBuilder();

            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["openTag"].Value);

            result.Append(match.Groups["ws1"].Value);

            patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["tagName"].Value);

            if (pattern.HighlightAttributes) {
                var highlightedAttributes = ProcessMarkupPatternAttributeMatches(pattern, match);
                result.Append(highlightedAttributes);
            }

            result.Append(match.Groups["ws5"].Value);

            patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.BracketColors, pattern.Style.Font);
            result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["closeTag"].Value);

            return result.ToString();
        }

        /// <summary>
        /// Processes a word type pattern element and returns the HTML output.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="pattern">Pattern to be processed</param>
        /// <param name="match">Result from regular expression to be highligted</param>
        protected override string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
        {
            var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.Style);

            return String.Format(StyleSpanFormat, patternStyle, match.Value);
        }

        /// <summary>
        /// Processes attributes from markup type pattern, creates styles for them and returns the HTML output..
        /// </summary>
        /// <param name="pattern">Pattern to be processed</param>
        /// <param name="match">Result from regular expression to be highligted</param>
        /// <returns></returns>
        private string ProcessMarkupPatternAttributeMatches(MarkupPattern pattern, Match match)
        {
            var result = new StringBuilder();

            for (var i = 0; i < match.Groups["attribute"].Captures.Count; i++) {
                result.Append(match.Groups["ws2"].Captures[i].Value);
                var patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeNameColors, pattern.Style.Font);
                result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["attribName"].Captures[i].Value);

                if (String.IsNullOrWhiteSpace(match.Groups["attribValue"].Captures[i].Value)) {
                    continue;
                }

                patternStyle = HtmlEngineHelper.CreatePatternStyle(pattern.AttributeValueColors, pattern.Style.Font);
                result.AppendFormat(StyleSpanFormat, patternStyle, match.Groups["attribValue"].Captures[i].Value);
            }

            return result.ToString();
        }
    }
}