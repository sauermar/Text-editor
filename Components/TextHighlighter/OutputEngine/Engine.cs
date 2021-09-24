using System;
using System.Linq;
using System.Text.RegularExpressions;
using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.OutputEngine
{
    [Sauerova]
    public abstract class Engine : IEngine
    {
        private const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace;

        public string Highlight(Definition definition, string input)
        {
            if (definition == null) {
                throw new ArgumentNullException("definition");
            }

            var output = PreHighlight(definition, input);
            output = HighlightUsingRegex(definition, output);
            output = PostHighlight(definition, output);

            return output;
        }

        /// <summary>
        /// Function for converting a string to the expected output before highlighting it.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="input">The string to be highlighted</param>
        /// <returns></returns>
        protected virtual string PreHighlight(Definition definition, string input)
        {
            return input;
        }

        /// <summary>
        /// Function for adding the appropriate styles to the input after highlighting.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="input">The string to be styled</param>
        /// <returns></returns>
        protected virtual string PostHighlight(Definition definition, string input)
        {
            return input;
        }

        /// <summary>
        /// Replaces all strings, which are being highligted using regular expression.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <param name="input">The string to be highlighted</param>
        /// <returns></returns>
        private string HighlightUsingRegex(Definition definition, string input)
        {
            var regexOptions = GetRegexOptions(definition);
            var evaluator = GetMatchEvaluator(definition);
            var regexPattern = definition.GetRegexPattern();
            var output = Regex.Replace(input, regexPattern, evaluator, regexOptions);

            return output;
        }

        /// <summary>
        /// Returns the appropriate regex options.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        /// <returns></returns>
        private RegexOptions GetRegexOptions(Definition definition)
        {
            if (definition.CaseSensitive) {
                return DefaultRegexOptions | RegexOptions.IgnoreCase;
            }

            return DefaultRegexOptions;
        }

        /// <summary>
        /// Handles the appropriate highligting process according to the pattern type from the configuration definition.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        private string ElementMatchHandler(Definition definition, Match match)
        {
            if (definition == null) {
                throw new ArgumentNullException("definition");
            }
            if (match == null) {
                throw new ArgumentNullException("match");
            }

            var pattern = definition.Patterns.First(x => match.Groups[x.Key].Success).Value;
            if (pattern != null) {
                if (pattern is BlockPattern) {
                    return ProcessBlockPatternMatch(definition, (BlockPattern) pattern, match);
                }
                if (pattern is MarkupPattern) {
                    return ProcessMarkupPatternMatch(definition, (MarkupPattern) pattern, match);
                }
                if (pattern is WordPattern) {
                    return ProcessWordPatternMatch(definition, (WordPattern) pattern, match);
                }
            }

            return match.Value;
        }

        /// <summary>
        /// Returns match evaluator.
        /// </summary>
        /// <param name="definition">The highlighting definition from configuration</param>
        private MatchEvaluator GetMatchEvaluator(Definition definition)
        {
            return match => ElementMatchHandler(definition, match);
        }

        /// <summary>
        /// Functions for higlighting individual types of patterns.
        /// </summary>
        protected abstract string ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match);
        protected abstract string ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match);
        protected abstract string ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match);
    }
}