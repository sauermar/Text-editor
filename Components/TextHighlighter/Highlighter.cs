using System;
using Components.TextHighlighter.Configuration;
using Components.TextHighlighter.OutputEngine;

namespace Components
{
    [Sauerova]
    public class Highlighter
    {
        public IEngine Engine { get; set; }
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Highlighter generic constructor.
        /// </summary>
        /// <param name="engine">Engine to be used for generating output</param>
        /// <param name="configuration">Configuration definitions for highlighting</param>
        public Highlighter(IEngine engine, IConfiguration configuration)
        {
            Engine = engine;
            Configuration = configuration;
        }

        /// <summary>
        /// Highlighter constructor with default configuration from XML document.
        /// </summary>
        /// <param name="engine">Engine to be used for generating output</param>
        public Highlighter(IEngine engine)
            : this(engine, new DefaultConfiguration())
        {
        }

        /// <summary>
        /// Main function for highlighting an input by highlighter with some configuration, using chosen definition.
        /// </summary>
        /// <param name="definitionName">Name of the definition to be used for highlighting from XML document</param>
        /// <param name="input">Input to be highlighted</param>
        /// <returns></returns>
        public string Highlight(string definitionName, string input)
        {
            if (definitionName == null)
            {
                throw new ArgumentNullException("definitionName");
            }

            if (Configuration.Definitions.ContainsKey(definitionName))
            {
                var definition = Configuration.Definitions[definitionName];
                return Engine.Highlight(definition, input);
            }

            return input;
        }
    }
}