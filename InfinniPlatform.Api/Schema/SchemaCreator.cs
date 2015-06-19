using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Schema
{
    public sealed class SchemaCreator
    {
        public dynamic BuildSchema(IEnumerable<SchemaObject> schemaObjects)
        {
            //конструируем объект из множества описаний элементов схемы
            var partialObject = new DataSchema();

            foreach (var schemaObject in schemaObjects)
            {
                FillDataSchema(partialObject, schemaObject);
            }

            return ((object) ProcessSchema(partialObject)).ToDynamic();
        }

        private void FillDataSchema(DataSchema partialObject, SchemaObject schemaObject)
        {
            var root = schemaObject;
            var members = new List<SchemaObject>();
            while (root != null)
            {
                members.Add(root);
                root = root.Parent;
            }

            members.Reverse();

            partialObject.Properties = partialObject.Properties ?? new Dictionary<string, DataSchema>();

            for (var i = 0; i < members.Count; i++)
            {
                if (partialObject.Properties.ContainsKey(members[i].Name))
                {
                    continue;
                }


                if (members[i].Parent == null)
                {
                    if (members[i].IsArray)
                    {
                        partialObject.Properties.Add(members[i].Name, new DataSchema
                        {
                            Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type),
                            Items = CreateItems(members[i])
                        });
                    }
                    else if (members[i].IsDocumentLink)
                    {
                        partialObject.Properties.Add(members[i].Name, new DataSchema
                        {
                            Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type),
                            Properties = CreateProperties(members[i])
                        });
                    }
                    else
                    {
                        partialObject.Properties.Add(members[i].Name, new DataSchema
                        {
                            Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type)
                        });
                    }
                }
                else
                {
                    var dataSchema = FindProperty(partialObject, members[i]);

                    if (members[i].IsArray)
                    {
                        dataSchema.Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type);
                        dataSchema.Items = CreateItems(members[i]);
                    }
                    else if (members[i].IsDocumentLink)
                    {
                        dataSchema.Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type);
                        dataSchema.Properties = CreateProperties(members[i]);
                    }
                    else
                    {
                        dataSchema.Type = SchemaDataTypeExtensions.ConvertFromString(members[i].Type);
                    }
                }
            }
        }

        private DataSchema CreateItems(SchemaObject schemaObject)
        {
            var result = new DataSchema();
            result.Type = SchemaDataTypeExtensions.ConvertFromString(schemaObject.ArrayItemType);
            if (schemaObject.IsDocumentArray)
            {
                result.Properties = CreateProperties(schemaObject);
            }
            return result;
        }

        private IDictionary<string, DataSchema> CreateProperties(SchemaObject schemaObject)
        {
            var properties = new Dictionary<string, DataSchema>();
            foreach (KeyValuePair<string, dynamic> property in schemaObject.ObjectSchema.Properties)
            {
                var propertyDataSchema = new DataSchema
                {
                    Type = SchemaDataTypeExtensions.ConvertFromString(property.Value.Type)
                };

                properties.Add(property.Key, propertyDataSchema);
            }
            properties.Add("Id", new DataSchema
            {
                Type = SchemaDataType.String
            });
            properties.Add("DisplayName", new DataSchema
            {
                Type = SchemaDataType.String
            });
            return properties;
        }

        private DataSchema FindProperty(DataSchema partialObject, SchemaObject findObject)
        {
            var path = findObject.ParentPath.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            path.AddRange(findObject.Name.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries));

            var schemaToFind = partialObject;

            var isCollectionItemPath = false;
            foreach (var s in path)
            {
                if (schemaToFind == null)
                {
                    return null;
                }


                if (s != "$")
                {
                    if (isCollectionItemPath)
                    {
                        schemaToFind.Items.Properties.TryGetValue(s, out schemaToFind);
                    }
                    else
                    {
                        schemaToFind.Properties.TryGetValue(s, out schemaToFind);
                    }
                }

                isCollectionItemPath = s == "$";

                //else
                //{
                //	schemaToFind = schemaToFind.Items;
                //}

                if (s == path.LastOrDefault())
                {
                    return schemaToFind;
                }
            }
            return partialObject;
        }

        /// <summary>
        ///     Выполняет постобработку схемы для замены IDictionary<string, DataSchema> на dynamic object
        /// </summary>
        private dynamic ProcessSchema(DataSchema sourceSchema)
        {
            dynamic updatedSchema = new
            {
                sourceSchema.Type,
                sourceSchema.Id,
                sourceSchema.Sortable
            }.ToDynamic();

            if (sourceSchema.Items != null)
            {
                updatedSchema.Items = ProcessSchema(sourceSchema.Items);
            }

            if (sourceSchema.Properties != null)
            {
                updatedSchema.Properties = new DynamicWrapper();

                foreach (var property in sourceSchema.Properties)
                {
                    updatedSchema.Properties[property.Key] = ProcessSchema(property.Value);
                }
            }

            return updatedSchema;
        }
    }
}