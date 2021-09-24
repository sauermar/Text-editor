using System.Xml.Linq;
using Components.Properties;

namespace Components.TextHighlighter.Configuration
{
    [Sauerova]
    public class DefaultConfiguration : XmlConfiguration
    {
        /// <summary>
        /// A default configuration defined by the XML document with default definitions, parsed by XmlConfiguration.
        /// </summary>
        public DefaultConfiguration()
        {
            XmlDocument = XDocument.Parse(Resources.DefaultDefinitions);
        }
    }
}