using System.Collections.Generic;
using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.Configuration
{
    public interface IConfiguration
    {
        IDictionary<string, Definition> Definitions { get; }
    }
}