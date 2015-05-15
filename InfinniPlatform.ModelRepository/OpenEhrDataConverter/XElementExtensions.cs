using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace InfinniPlatform.ModelRepository.OpenEhrDataConverter
{
    /// <summary>
    /// Provides methods for loading and parcing xml documents
    /// </summary>
    internal static class XElementExtensions
    {
        /// <summary>
        /// Extracts a specified attribute and converts it to desired type
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="source">Data source</param>
        /// <param name="name">Attribute name</param>
        /// <param name="value">returned value</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>False is extraction failed</returns>
        internal static bool ExtractAttributeValue<T>(
            this XElement source,
            XName name,
            out T value,
            out string errorMessage) where T : IConvertible
        {
            var result = true;
            errorMessage = string.Empty;
            value = default(T);
            var attibute = source.Attribute(name);
            if (attibute == null)
            {
                result = false;
                errorMessage = string.Format("Expected attibute {0} is not found", name);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(attibute.Value))
                {
                    result = false;
                    errorMessage = string.Format("Invalid attribute value {0}", name);
                }
                else
                {
                    try
                    {
                        value = (T) Convert.ChangeType(attibute.Value, typeof (T), CultureInfo.CurrentCulture);
                    }
                    catch (Exception e)
                    {
                        result = false;
                        errorMessage = string.Format(
                            "Atrribute value conversion fail of the {0} conversion. {1}",
                            name,
                            e.Message);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts a value of specified element and converts it to desired type
        /// </summary>
        /// <typeparam name="T">Desired type</typeparam>
        /// <param name="parentSource">Data source</param>
        /// <param name="value">returned value</param>
        /// <param name="nodes">Child element names</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>False is extraction failed</returns>
        internal static bool ExtractElementValueByXPath<T>(
            this XElement parentSource,
            out T value,
            out string errorMessage,
            params XName[] nodes) where T : IConvertible
        {
            errorMessage = string.Empty;
            value = default(T);
            
            var childElement = parentSource;

            foreach (var node in nodes)
            {
                childElement = childElement.Element(node);
                if (childElement == null)
                {
                    errorMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        "Expected element {0} is not found under path {1}", 
                        node, 
                        string.Join("/", nodes.Select(n => n.NamespaceName)));
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(childElement.Value))
            {
                errorMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    "Invalid element value {0}", 
                    string.Join("/", nodes.Select(n => n.NamespaceName)));
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(childElement.Value, typeof(T), CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                errorMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    "Element value conversion fail of the {0} conversion. {1}",
                    string.Join("/", nodes.Select(n => n.NamespaceName)),
                    e.Message);
                return false;
            }

            return true;
        }
    }
}