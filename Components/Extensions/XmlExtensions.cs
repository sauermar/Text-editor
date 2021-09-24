using System;
using System.Xml.Linq;

namespace Components.Extensions
{
    [Sauerova]
    internal static class XmlExtensions
    {
        /// <summary>
        /// Returns an attribute value from xml element.
        /// </summary>
        /// <param name="element">XML element with attribute</param>
        /// <param name="name">The name of the attribute</param>
        /// <returns></returns>
        public static string GetAttributeValue(this XElement element, XName name)
        {
            if (element == null) {
                throw new ArgumentNullException("element");
            }

            var attribute = element.Attribute(name);
            if (attribute == null) {
                return null;
            }

            return attribute.Value;
        }
    }
}