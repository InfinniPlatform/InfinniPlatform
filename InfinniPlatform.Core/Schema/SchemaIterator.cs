using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Core.Schema
{
    public sealed class SchemaIterator
    {
        private readonly IList<LinkEntry> _entries = new List<LinkEntry>();
        private readonly ISchemaProvider _schemaProvider;
        private readonly List<dynamic> _typeInfoChain = new List<dynamic>();

        public SchemaIterator(ISchemaProvider schemaProvider)
        {
            _schemaProvider = schemaProvider;
        }

        public Action<SchemaObject> OnPrimitiveProperty { get; set; }
        public Action<SchemaObject> OnObjectProperty { get; set; }
        public Action<SchemaObject> OnArrayProperty { get; set; }

        private dynamic GetDocumentSchema(string configuration, string document)
        {
            if (!string.IsNullOrEmpty(configuration) &&
                !string.IsNullOrEmpty(document))
            {
                return _schemaProvider.GetSchema(configuration, document);
            }
            return null;
        }

        public void ProcessSchema(dynamic schema)
        {
            _entries.Clear();
            ProcessSchema((SchemaObject)null, schema);
        }

        private void ProcessSchema(SchemaObject parentInfo, dynamic schema)
        {
            if (schema == null)
            {
                return;
            }

            foreach (var startInfo in schema.Properties)
            {
                //пока не сделано предзаполнение свойств-массивов
                var propertyValue = startInfo.Value;
                if (propertyValue.Type == "Object")
                {
                    if (propertyValue.TypeInfo != null && propertyValue.TypeInfo.DocumentLink != null)
                    {
                        dynamic schemaInner = GetDocumentSchema(propertyValue.TypeInfo.DocumentLink.ConfigId,
                            propertyValue.TypeInfo.DocumentLink.DocumentId);

                        var linkSchemaObject = new SchemaObject(parentInfo, startInfo.Key, propertyValue.Caption,
                            startInfo.Value, schemaInner);

                        if (_typeInfoChain.Any(t => t.ConfigId == propertyValue.TypeInfo.DocumentLink.ConfigId &&
                                                    t.DocumentId == propertyValue.TypeInfo.DocumentLink.DocumentId))
                        {
                            continue;
                        }

                        _typeInfoChain.Add(propertyValue.TypeInfo.DocumentLink);

                        if (OnObjectProperty != null)
                        {
                            OnObjectProperty(linkSchemaObject);
                        }

                        //добавляем свойство с идентификатором объекта (которое отсутствует в описаниях моделей)
                        var schemaId = new SchemaObject(linkSchemaObject, "Id", "Id", null, schemaInner);
                        if (OnPrimitiveProperty != null)
                        {
                            OnPrimitiveProperty(schemaId);
                        }


                        if (propertyValue.TypeInfo.DocumentLink.Inline == null ||
                            propertyValue.TypeInfo.DocumentLink.Inline == false)
                        {
                            var schemaDisplayProperty = new SchemaObject(linkSchemaObject, "DisplayName", "DisplayName",
                                null, schemaInner);
                            if (OnPrimitiveProperty != null)
                            {
                                OnPrimitiveProperty(schemaDisplayProperty);
                            }
                        }

                        //---
                        var linkEntry = new LinkEntry
                        {
                            ConfigId = propertyValue.TypeInfo.DocumentLink.ConfigId,
                            DocumentId = propertyValue.TypeInfo.DocumentLink.DocumentId,
                            Schema = schemaInner
                        };

                        if (!_entries.HasEntry(linkEntry))
                        {
                            _entries.Add(linkEntry);

                            if (linkSchemaObject.Inline)
                            {
                                ProcessSchema(linkSchemaObject, schemaInner);
                            }
                        }
                        //---
                    }
                }
                else if (propertyValue.Type == "Array")
                {
                    if (OnArrayProperty != null)
                    {
                        //если массив содержит элементы типа "объект" то получаем метаданные объекта данного типа
                        dynamic schemaObject = null;
                        if (propertyValue.Items.Type == "Object" && propertyValue.Items.TypeInfo != null &&
                            propertyValue.Items.TypeInfo.DocumentLink != null)
                        {
                            schemaObject = GetDocumentSchema(propertyValue.Items.TypeInfo.DocumentLink.ConfigId,
                                propertyValue.Items.TypeInfo.DocumentLink.DocumentId);
                        }

                        //обрабатываем сам массив
                        var arraySchemaObject = new SchemaObject(parentInfo, startInfo.Key, propertyValue.Caption,
                            startInfo.Value,
                            schemaObject);

                        OnArrayProperty(arraySchemaObject);

                        //если массив содержит ссылочные типы, то выполняем анализ типа элемента массива
                        if (schemaObject != null)
                        {
                            var linkEntry = new LinkEntry
                            {
                                ConfigId = propertyValue.Items.TypeInfo.DocumentLink.ConfigId,
                                DocumentId = propertyValue.Items.TypeInfo.DocumentLink.DocumentId,
                                Schema = schemaObject
                            };

                            var linkToObjectIsCycleReference = _entries.HasEntry(linkEntry);
                            if (!linkToObjectIsCycleReference)
                            {
                                _entries.Add(linkEntry);
                                ProcessSchema(arraySchemaObject, schemaObject);
                            }
                        }
                    }
                }
                else
                {
                    if (OnPrimitiveProperty != null)
                    {
                        OnPrimitiveProperty(new SchemaObject(parentInfo, startInfo.Key, propertyValue.Caption,
                            startInfo.Value, null));
                    }
                }
            }
        }
    }
}