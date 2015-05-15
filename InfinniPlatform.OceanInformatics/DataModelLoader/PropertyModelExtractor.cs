using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;
using InfinniPlatform.Extensions;

using OpenEhr.V1.Its.Xml.AM;

using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    internal sealed class PropertyModelExtractor
    {
        private readonly string _configId;

        internal PropertyModelExtractor(string configId)
        {
            _configId = configId;
        }

        internal KeyValuePair<string, DataSchema> Extract(C_COMPLEX_OBJECT property, OntologiesProvider ontologies)
        {
            var propertyName = ontologies.GetText(property.node_id);
            var propertyDescription = ontologies.GetDescription(property.node_id);

            // method will change property name only if it is redefined in child attributes
            propertyName = TryToGetPropertyNameFromChildAttributes(property, propertyName);

            var propertyType = property.rm_type_name;
            if (property.attributes != null && 
                property.attributes.Length > 0 &&
                property.attributes[0].children.Length > 0)
            {
                propertyType = property.attributes[0].children[0].rm_type_name;
            }

            string nameToReturn = propertyName.ToTranslit();

            var modelProperty = new DataSchema
            {
                Description = propertyDescription,
                Caption = propertyName,
                Type = DataType.Object.ToString()
            };

            if (!property.occurrences.upperSpecified ||
                (property.occurrences.upperSpecified && 
                property.occurrences.upper != 1))
            {
                // это означает, что свойство является массивом
                var arrayItem = new DataSchema
                {
                    Type = DataType.Object.ToString()
                };

                arrayItem.TypeInfo.Add("DocumentLink", new DocumentLink
                {
                    ConfigId = _configId,
                    DocumentId = propertyType,
                    Inline = true
                });

                modelProperty = new DataSchema
                {
                    Description = propertyDescription,
                    Caption = propertyName,
                    Type = DataType.Array.ToString(),
                    Items = arrayItem
                };
            }
            else
            {
                modelProperty.TypeInfo.Add("DocumentLink", new DocumentLink
                {
                    ConfigId = _configId,
                    DocumentId = propertyType,
                    Inline = true
                });
            }

            modelProperty.TypeInfo.Add("NodeId", property.node_id);

            if (propertyType == "DV_CODED_TEXT")
            {
                ExtractListOfPossibleValues(property, ontologies, modelProperty);
            }

            ExtractValidationRules(property, ontologies, modelProperty);

            return new KeyValuePair<string, DataSchema>(nameToReturn, modelProperty);
        }

        private static string TryToGetPropertyNameFromChildAttributes(C_COMPLEX_OBJECT property, string propertyName)
        {
            // property name can also be located in the special attribute
            if (property.attributes.Length == 2 &&
                property.attributes[0].rm_attribute_name == "value" &&
                property.attributes[1].rm_attribute_name == "name")
            {
                // property it is a pair of attributes: value and name
                var nameAttributes = ((C_COMPLEX_OBJECT) property.attributes[1].children[0]).attributes;
                if (nameAttributes != null && nameAttributes.Length > 0)
                {
                    var nameDefinition = nameAttributes[0].children;
                    if (nameDefinition != null && nameDefinition.Length > 0 && nameDefinition[0] is C_PRIMITIVE_OBJECT)
                    {
                        var item = ((C_PRIMITIVE_OBJECT) nameDefinition[0]).item;
                        var stringList = item as C_STRING;
                        if (stringList != null)
                        {
                            var strings = stringList.list;
                            if (strings != null && strings.Length > 0)
                            {
                                propertyName = strings[0];
                            }
                        }
                    }
                }
            }

            return propertyName;
        }

        private void ExtractListOfPossibleValues(C_COMPLEX_OBJECT property, OntologiesProvider ontologies, DataSchema modelProperty)
        {
            if (property.attributes != null && property.attributes.Length > 0)
            {
                var children = property.attributes[0].children;
                if (children != null && children.Length > 0 && children[0] is C_COMPLEX_OBJECT)
                {
                    var childrenAttributes = ((C_COMPLEX_OBJECT)children[0]).attributes;
                    if (childrenAttributes != null && childrenAttributes.Length > 0 && childrenAttributes[0].children.Length > 0)
                    {
                        var attributeDefinition = childrenAttributes[0].children[0];
                        if (attributeDefinition != null)
                        {
                            var constraintRef = attributeDefinition as CONSTRAINT_REF;
                            if (constraintRef != null)
                            {
                                // TODO: investigate what is constraintRef and how to use it
                                modelProperty.TypeInfo.Add("ConstantRef", constraintRef.reference);
                            }

                            var codePhrases = attributeDefinition as C_CODE_PHRASE;
                            if (codePhrases != null)
                            {
                                if (codePhrases.code_list != null)
                                {
                                    modelProperty.TypeInfo.Add("CodeList", codePhrases.code_list.Select(ontologies.GetText).ToList());
                                }
                                else
                                {
                                    var codeReference = codePhrases as C_CODE_REFERENCE;
                                    if (codeReference != null)
                                    {
                                        // It is a link to classifier
                                        modelProperty.Type = DataType.Object.ToString();
                                        modelProperty.TypeInfo["ReferenceLink"] = new DocumentLink
                                        {
                                            ConfigId = _configId,
                                            DocumentId = codeReference.referenceSetUri,
                                            Inline = false
                                        };
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ExtractValidationRules(C_COMPLEX_OBJECT property, OntologiesProvider ontologies, DataSchema modelProperty)
        {
            /* TODO: refactoring required: There should be used validation rules instead of attributes
             * 
             * Postponed until validation requirements will be clearified
                 
            if (propertyType == "DV_QUANTITY")
            {                    
                    var quantityItem = ((C_DV_QUANTITY)property.attributes[0].children[0]).list[0];
                    if(!string.IsNullOrEmpty(quantityItem.units))
                    {
                        modelProperty.Attributes[PropertyAttributeName.Units.ToString()] =
                            JToken.FromObject(quantityItem.units);
                    }
                        
                    if (quantityItem.magnitude != null)
                    {
                        if (quantityItem.magnitude.lowerSpecified)
                        {
                            modelProperty.Attributes[PropertyAttributeName.LowerLimit.ToString()] =
                                JToken.FromObject(quantityItem.magnitude.lower);
                        }

                        if (quantityItem.magnitude.upperSpecified)
                        {
                            modelProperty.Attributes[PropertyAttributeName.UpperLimit.ToString()] =
                                JToken.FromObject(quantityItem.magnitude.upper);
                        }
                    }                         

                    if (quantityItem.precision != null)
                    {
                        if (quantityItem.precision.upperSpecified)
                        {
                            modelProperty.Attributes[PropertyAttributeName.Precision.ToString()] =
                                JToken.FromObject(quantityItem.precision.upper);
                        }
                    }                   
            }
            else if (propertyType == "DV_COUNT")
            {                                            
                var interval = ((C_INTEGER)(((C_PRIMITIVE_OBJECT)((((C_COMPLEX_OBJECT)
                (property.attributes[0].children[0])).attributes[0]).children[0])).item)).range;
                        
                if (interval != null)
                {
                    if (interval.lowerSpecified)
                    {
                        modelProperty.Attributes[PropertyAttributeName.LowerLimit.ToString()] =
                            JToken.FromObject(interval.lower);
                    }

                    if (interval.upperSpecified)
                    {
                        modelProperty.Attributes[PropertyAttributeName.UpperLimit.ToString()] =
                            JToken.FromObject(interval.upper);
                    }
                } 
            }*/
        }
    }
}
