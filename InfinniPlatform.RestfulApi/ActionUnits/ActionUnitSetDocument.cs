using System;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Logging;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitSetDocument
    {
        private static IMetadataComponent _metadataComponent;

        public void Action(IApplyContext target)
        {
            var version = target.Context.GetVersion(target.Item.Configuration, target.UserName);

            dynamic documentProvider =
                target.Context.GetComponent<InprocessDocumentComponent>()
                      .GetDocumentProvider(version, target.Item.Configuration, target.Item.Metadata,
                          target.UserName);

            dynamic instanceId = null;
            if (documentProvider != null)
            {
                TransactionUtils.ApplyTransactionMarker(target);

                _metadataComponent = target.Context.GetComponent<IMetadataComponent>();

                IEnumerable<dynamic> result = _metadataComponent.GetMetadataList(version,
                    target.Item.Configuration,
                    target.Item.Metadata,
                    MetadataType.Schema);

                dynamic documentSchema = result.FirstOrDefault();

                bool allowNonSchemaProperties = target.Item.AllowNonSchemaProperties != null &&
                                                target.Item.AllowNonSchemaProperties == true;

                if (target.Item.Document != null)
                {
                    target.Item.Document = AdjustDocumentContent(version, target.Item.Document, documentSchema,
                        allowNonSchemaProperties);

                    instanceId = target.Item.Document.Id;
                }
                else if (target.Item.Documents != null)
                {
                    var adjustedDocs = new List<dynamic>();

                    foreach (var document in target.Item.Documents)
                    {
                        adjustedDocs.Add(AdjustDocumentContent(version, document, documentSchema,
                            allowNonSchemaProperties));
                    }

                    target.Item.Documents = adjustedDocs.ToArray();
                }
                target.Result = new DynamicWrapper();

                target.Result.IsValid = true;
                target.Result.ValidationMessage = string.Empty;

                if (instanceId != null)
                {
                    target.Result.Id = instanceId;
                }

                return;
            }


            var log = target.Context.GetComponent<ILogComponent>().GetLog();

            log.Warn("Document type does not exist.", new Dictionary<string, object>
                                                      {
                                                          { "configuration", target.Item.Configuration },
                                                          { "type", target.Item.Metadata },
                                                      });

            // throw new ArgumentException(string.Format("document \"{0}\" type \"{1}\" not exists", target.Item.Configuration, target.Item.Metadata));
        }

        /// <summary>
        ///     Перед сохранением документа его нужно подготовить:
        ///     1. Добавление поля Id (если его нет)
        ///     2. Удаление полей, несоответствующих схеме документа
        /// </summary>
        private static dynamic AdjustDocumentContent(string version, dynamic document, dynamic documentSchema,
                                                     bool allowNonSchemaProperties)
        {
            if (document.Id == null)
            {
                document.Id = Guid.NewGuid().ToString();
            }

            if (documentSchema != null && !allowNonSchemaProperties)
            {
                return RemoveInappropriateProperties(version, document, documentSchema);
            }

            return document;
        }

        /// <summary>
        ///     Удаление свойств, несоответствующих схеме документа
        /// </summary>
        private static dynamic RemoveInappropriateProperties(string version, dynamic document, dynamic schema)
        {
            if (document == null)
            {
                return null;
            }

            var propertyNamesToRemove = new List<string>();
            var propertiesToModify = new Dictionary<string, dynamic>();

            dynamic documentClone = document.Clone();

            foreach (dynamic documentProperty in documentClone)
            {
                // Данные поля могут присутствовать в документе, даже если их нет в схеме
                if (documentProperty.Key == "Id" || documentProperty.Key == "DisplayName")
                {
                    continue;
                }

                dynamic propertyMetadata = schema.Properties[documentProperty.Key];


                if (propertyMetadata == null)
                {
                    propertyNamesToRemove.Add(documentProperty.Key);
                }
                else
                {
                    if (propertyMetadata.Type == "Object")
                    {
                        propertiesToModify.Add(
                            documentProperty.Key,
                            ProcessInnerObjectProperties(version, propertyMetadata.TypeInfo, documentProperty));
                    }
                    else if (propertyMetadata.Type == "Array")
                    {
                        if (propertyMetadata.Items == null)
                        {
                            continue;
                        }

                        propertiesToModify.Add(
                            documentProperty.Key,
                            ProcessInnerArrayProperties(version, propertyMetadata.Items.TypeInfo, documentProperty));
                    }
                }
            }

            foreach (var propertyName in propertyNamesToRemove)
            {
                documentClone[propertyName] = null;
            }

            foreach (var propertyToModify in propertiesToModify)
            {
                documentClone[propertyToModify.Key] = propertyToModify.Value;
            }

            return documentClone;
        }

        /// <summary>
        ///     Обработка вложенных свойств объектов
        /// </summary>
        private static dynamic ProcessInnerObjectProperties(string version, dynamic propertyTypeInfo, dynamic documentProperty)

        {
            dynamic result = documentProperty.Value;

            if (propertyTypeInfo == null)
            {
                return result;
            }

            if (propertyTypeInfo.DocumentLink != null)
            {
                if (propertyTypeInfo.DocumentLink.Inline == true)
                {
                    dynamic documentSchema = null;
                    if (propertyTypeInfo.DocumentLink.ConfigId != null &&
                        propertyTypeInfo.DocumentLink.DocumentId != null)
                    {
                        IEnumerable<dynamic> metadataList = _metadataComponent.GetMetadataList(version,
                            propertyTypeInfo.DocumentLink.ConfigId,
                            propertyTypeInfo.DocumentLink.DocumentId,
                            MetadataType.Schema);

                        documentSchema = metadataList.FirstOrDefault();
                    }

                    if (documentSchema != null)
                    {
                        result = RemoveInappropriateProperties(version, documentProperty.Value, documentSchema);
                    }
                }
                else
                {
                    if (documentProperty.Value != null &&
                        propertyTypeInfo.DocumentLink.ConfigId != null &&
                        propertyTypeInfo.DocumentLink.DocumentId != null)
                    {
                        RemoveLinkExtraProperties(documentProperty.Value);
                        result = documentProperty.Value;
                    }
                }
            }
            else if (propertyTypeInfo.BinaryLink != null)
            {
                // TODO
            }
            else
            {
                result = RemoveInappropriateProperties(version, documentProperty.Value, propertyTypeInfo);
            }

            return result;
        }

        /// <summary>
        ///     Обработка вложенных свойств массивов
        /// </summary>
        private static IEnumerable<dynamic> ProcessInnerArrayProperties(string version, dynamic propertyTypeInfo,
                                                                        dynamic documentProperty)
        {
            var handledItems = new List<dynamic>();

            if (propertyTypeInfo == null)
            {
                return documentProperty.Value;
            }

            if (propertyTypeInfo.DocumentLink != null)
            {
                if (propertyTypeInfo.DocumentLink.Inline == true)
                {
                    dynamic documentSchema = null;
                    if (propertyTypeInfo.DocumentLink.ConfigId != null &&
                        propertyTypeInfo.DocumentLink.DocumentId != null)
                    {
                        IEnumerable<dynamic> metadataList = _metadataComponent.GetMetadataList(version,
                            propertyTypeInfo.DocumentLink.ConfigId,
                            propertyTypeInfo.DocumentLink.DocumentId,
                            MetadataType.Schema);


                        documentSchema = metadataList.FirstOrDefault();
                    }


                    if (documentSchema != null)
                    {
                        foreach (var documentValue in DynamicWrapperExtensions.ToEnumerable(documentProperty.Value))
                        {
                            handledItems.Add(RemoveInappropriateProperties(version, documentValue, documentSchema));
                        }
                    }
                }
                else
                {
                    foreach (var documentValue in DynamicWrapperExtensions.ToEnumerable(documentProperty.Value))
                    {
                        RemoveLinkExtraProperties(documentValue);
                    }

                    handledItems = documentProperty.Value;
                }
            }
            else if (propertyTypeInfo.BinaryLink != null)
            {
                // TODO
            }
            else
            {
                foreach (var documentValue in DynamicWrapperExtensions.ToEnumerable(documentProperty.Value))
                {
                    handledItems.Add(RemoveInappropriateProperties(version, documentValue, propertyTypeInfo));
                }
            }

            return handledItems;
        }

        private static void RemoveLinkExtraProperties(dynamic instanceToHandle)
        {
            var propertyNames = new List<string>();

            foreach (dynamic documentProp in instanceToHandle)
            {
                if (documentProp.Key != "Id" &&
                    documentProp.Key != "DisplayName")
                {
                    propertyNames.Add(documentProp.Key);
                }
            }

            foreach (var propertyName in propertyNames)
            {
                instanceToHandle[propertyName] = null;
            }
        }
    }
}