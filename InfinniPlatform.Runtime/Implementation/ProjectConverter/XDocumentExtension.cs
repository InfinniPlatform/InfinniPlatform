using System.Linq;
using System.Xml.Linq;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    static class XDocumentExtension
    {
        public static string GetAttributeValue(this XElement parent, string attributeName)
        {
            return (parent != null && parent.Attribute(attributeName) != null) ? parent.Attribute(attributeName).Value : null;
        }

        public static string GetElementValue(this XElement parent, XName tagName)
        {
            if (parent != null && parent.Element(tagName) != null)
                return parent.Element(tagName).Value;
            return null;
        }

        public static string GetDescendantValue(this XElement parent, XName tagName)
        {
            if (parent != null && parent.Descendants(tagName).Any())
                return parent.Descendants(tagName).First().Value;
            return null;
        }

        public static string GetElementValue(this XElement parent, string tagName, XNamespace ns)
        {
            return GetElementValue(parent, ns + tagName);
        }

        public static string GetDescendantValue(this XElement parent, string tagName, XNamespace ns)
        {
            return GetDescendantValue(parent, ns + tagName);
        }
    }
}
