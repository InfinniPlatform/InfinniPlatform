using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.ModelRepository.OpenEhrDataConverter
{
    internal sealed class PropertyDataExtractor
    {
        private readonly StringBuilder _errorKeeper;
        private readonly IDictionary<string, DataSchema> _inlineDocuments;
        private readonly string _defaultNamespace;
        private readonly string _xsiNamespace;

        internal PropertyDataExtractor(
            StringBuilder errorKeeper,
            IDictionary<string, DataSchema> inlineDocuments, 
            string defaultNamespace, 
            string xsiNamespace)
        {
            _errorKeeper = errorKeeper;
            _inlineDocuments = inlineDocuments;
            _defaultNamespace = defaultNamespace;
            _xsiNamespace = xsiNamespace;
        }

        internal JObject ExtractData(XElement content, DataSchema propertyModel)
        {
            string stringValue;
            string errorMessage;
            JObject propertyValue = null;

            var isTypeExtracted = content.ExtractAttributeValue(XName.Get("valueType"), out stringValue, out errorMessage);

            var valueElement = content.Element(XName.Get("value", _defaultNamespace));

            if (valueElement == null)
            {
                // Такая ситуация возможна, если данные поля не введены 
                // null flavour
                return new JObject(); 
            }

            if (!isTypeExtracted)
            {
                if (valueElement.ExtractAttributeValue(XName.Get("type", _xsiNamespace), out stringValue, out errorMessage))
                {
                    isTypeExtracted = true;
                }
            }

            if (isTypeExtracted)
            {
                // validate data type with model
                if (propertyModel.Type == DataType.Object.ToString())
                {
                    string modelTypeName = propertyModel.TypeInfo["DocumentLink"].ToDynamic()["DocumentId"].ToString();
                    
                    if (modelTypeName != stringValue)
                    {
                        _errorKeeper.AppendLine();
                        _errorKeeper.AppendFormat(
                            "Data type for property {0} doen't match. Expected {1}, but was {2}",
                            propertyModel.Caption, modelTypeName, stringValue);
                    }
                }

                if (_inlineDocuments.ContainsKey(stringValue))
                {
                    propertyValue = ExtractComplexTypeValue(valueElement, _inlineDocuments[stringValue]);
                }
                else
                {
                    _errorKeeper.AppendLine();
                    _errorKeeper.AppendFormat("Unknown data type {0} of element {1}", stringValue, propertyModel.Caption);
                }
            }
            else
            {
                _errorKeeper.AppendLine();
                _errorKeeper.Append(errorMessage);
            }

            return propertyValue;
        }

        private JObject ExtractComplexTypeValue(XElement valueElement, DataSchema complexType)
        {
            var propertyValue = new JObject();
            
            foreach (var property in complexType.Properties)
            {
                var childElement = valueElement.Element(XName.Get(property.Key, "http://schemas.openehr.org/v1"));
                if (childElement != null)
                {
                    if (property.Value.Type == DataType.Object.ToString())
                    {
                        if (property.Value.TypeInfo.ContainsKey("DocumentLink"))
                        {
                            var complexTypeName = ((DocumentLink) property.Value.TypeInfo["DocumentLink"]).DocumentId;

                            var complexTypeUnderLink = _inlineDocuments[complexTypeName];
                            propertyValue.Add(property.Key, ExtractComplexTypeValue(childElement, complexTypeUnderLink));
                        }
                    }
                    else
                    {
                        propertyValue.Add(property.Key, ExtractPropertyValue(childElement, property.Value));
                    }
                }
            }

            return propertyValue;
        }

        private JValue ExtractPropertyValue(XElement valueElement, DataSchema propertyModel)
        {
            object propertyValue = null;

            switch ((DataType)Enum.Parse(typeof(DataType), propertyModel.Type))
            {
                case DataType.String:
                    propertyValue = valueElement.Value;
                    break;
                case DataType.Bool:
                    propertyValue = bool.Parse(valueElement.Value);
                    break;
                case DataType.DateTime:
                    propertyValue = DateTime.Parse(valueElement.Value);
                    break;
                case DataType.Float:
                    propertyValue = float.Parse(valueElement.Value);
                    break;
                case DataType.Integer:
                    propertyValue = int.Parse(valueElement.Value);
                    break;
                default:
                    _errorKeeper.AppendLine();
                    _errorKeeper.AppendFormat("Unknown data type {0} of element {1}", propertyModel.Type, propertyModel.Caption);
                    break;
            }

            return new JValue(propertyValue);
        }
    }
}



