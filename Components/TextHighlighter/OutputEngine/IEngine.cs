using Components.Models.HighlightingPatterns;

namespace Components.TextHighlighter.OutputEngine
{
    [Sauerova]
    public interface IEngine
    {
        string Highlight(Definition definition, string input);
    }
}