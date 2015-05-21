using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;
using InfinniPlatform.Extensions;

using OpenEhr.V1.Its.Xml.AM;

using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    internal sealed class ObjectModelExtractor
    {
        private readonly PropertyModelExtractor _modelPropertyExtractor;
        private readonly bool _isArchetypesExtraction;
        private readonly string _configId;

        internal ObjectModelExtractor(bool isArchetypesExtraction, string configId)
        {
            _modelPropertyExtractor = new PropertyModelExtractor(configId);
            _isArchetypesExtraction = isArchetypesExtraction;
            _configId = configId;
        }

        internal KeyValuePair<string, DataSchema> Extract(C_COMPLEX_OBJECT propertyGroup, OntologiesProvider ontologies)
        {
            var nameToReturn = ontologies.GetText(propertyGroup.node_id).ToTranslit();

            var modelPropertyGroup = new DataSchema
            {
                Type = DataType.Object.ToString(),
                Description = ontologies.GetDescription(propertyGroup.node_id),
                Caption = ontologies.GetText(propertyGroup.node_id),
            };
            
            var cArchetypeRoot = propertyGroup as C_ARCHETYPE_ROOT;

            modelPropertyGroup.TypeInfo.Add("NodeId",
                cArchetypeRoot != null ? cArchetypeRoot.archetype_id.value : propertyGroup.node_id);

            if (propertyGroup.attributes!=null)
            {
                foreach (var obj in propertyGroup.attributes.SelectMany(attribute => attribute.children))
                {
                    if (obj.rm_type_name == "ELEMENT")
                    {
                        var complexElement = obj as C_COMPLEX_OBJECT;

                        if (complexElement != null)
                        {
                            var extractedData = _modelPropertyExtractor.Extract(complexElement, ontologies);

                            if (!modelPropertyGroup.Properties.ContainsKey(extractedData.Key))
                                modelPropertyGroup.Properties.Add(extractedData);
                        }
                        else
                        {
                            var archetypeRef = obj as ARCHETYPE_INTERNAL_REF;
                            if (archetypeRef != null && _isArchetypesExtraction)
                            {
                                var refArchetype = new DataSchema
                                {
                                    Type = DataType.Object.ToString()
                                };

                                refArchetype.TypeInfo.Add("DocumentLink", new DocumentLink
                                {
                                    ConfigId = _configId,
                                    DocumentId = archetypeRef.target_path,
                                    Inline = true
                                });

                                // Свойства с именем ARCHETYPE_INTERNAL_REF являются ссылками
                                // на элементы в рамках текущего архетипа.
                                // Подобные ссылки будут разрешены после загрузки всего архетипа.
                                modelPropertyGroup.Properties.Add("ARCHETYPE_INTERNAL_REF_" + archetypeRef.target_path, refArchetype);
                            }
                        }
                    }
                    else if (obj.rm_type_name == "DV_TEXT")
                    {
                        // it is a redefinition for group name
                        var complexSubObject = obj as C_COMPLEX_OBJECT;
                        if (complexSubObject != null &&
                            complexSubObject.attributes.Length > 0 &&
                            complexSubObject.attributes[0].children.Length > 0)
                        {
                            var primitiveObject = complexSubObject.attributes[0].children[0] as C_PRIMITIVE_OBJECT;
                            if (primitiveObject != null &&
                                primitiveObject.item is C_STRING)
                            {
                                var strings = ((C_STRING)primitiveObject.item).list;
                                if (strings != null && strings.Length > 0)
                                {
                                    nameToReturn = strings[0].ToTranslit();
                                    modelPropertyGroup.Caption = strings[0];
                                }
                            }
                        }
                    }
                    else if (obj.rm_type_name == "EVENT_CONTEXT")
                    {
                        // Unclear that to do with this type of item. Just skip it for now
                    }
                    else if (obj.rm_type_name == "DV_CODED_TEXT")
                    {
                        // constant property. Just skip it for now
                    }
                    else if (obj.rm_type_name == "STRING")
                    {
                        // it is an additional attribute. Just skip it for now
                    }
                    else
                    {
                        var archetypeSlot = obj as ARCHETYPE_SLOT;
                        if (archetypeSlot != null && _isArchetypesExtraction)
                        {
                            // It is a link to other archetype
                            var propertyName = ontologies.GetText(obj.node_id).ToTranslit();

                            if (!modelPropertyGroup.Properties.ContainsKey(propertyName))
                            {
                                modelPropertyGroup.Properties.Add(
                                                                  propertyName,
                                                                  new DataSchema
                                                                  {
                                                                      Description = obj.rm_type_name,
                                                                      Caption = ontologies.GetText(obj.node_id),
                                                                      Type = DataType.Object.ToString()
                                                                  });
                            }
                        }

                        var complexObject = obj as C_COMPLEX_OBJECT;
                        if (complexObject != null)
                        {
                            var archetypeRoot = obj as C_ARCHETYPE_ROOT;
                            if (archetypeRoot != null)
                            {
                                modelPropertyGroup.Properties.Add(Extract(complexObject,
                                                                          new OntologiesProvider(archetypeRoot.term_definitions)));
                            }
                            else
                            {
                                var extractedData = Extract(complexObject, ontologies);
                                if (!modelPropertyGroup.Properties.ContainsKey(extractedData.Key))
                                    modelPropertyGroup.Properties.Add(extractedData);
                            }
                        }
                    }
                }
            }

            if (!propertyGroup.occurrences.upperSpecified ||
                (propertyGroup.occurrences.upperSpecified &&
                 propertyGroup.occurrences.upper != 1))
            {
                // следовательно, имеем дело с массивом
                modelPropertyGroup.Type = DataType.Array.ToString();
                modelPropertyGroup.Items = new DataSchema
                {
                    Type = DataType.Object.ToString(),
                    Properties = modelPropertyGroup.Properties
                };
                modelPropertyGroup.Properties = null;
            }

            return new KeyValuePair<string, DataSchema>(nameToReturn, modelPropertyGroup);
        }
    }
}
