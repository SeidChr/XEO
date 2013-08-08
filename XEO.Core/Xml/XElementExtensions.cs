using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XEO.Core.Xml
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Retrieves the value of an attribute or empty string if attribute was not found.
        /// </summary>
        /// <param name="element">The element to find the attribute on.</param>
        /// <param name="attributeName">Name of the attribute to find.</param>
        /// <returns>Empty string or value of the attribute.</returns>
        public static string TryGetAttributeValue(this XElement element, string attributeName)
        {
            var value = string.Empty;
            var attribute = element.Attribute(attributeName);
            
            if (attribute != null)
            {
                value = attribute.Value;
            }

            return value;
        }
    }
}
