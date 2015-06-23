using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitSetDocument
    {
        private static IMetadataComponent _metadataComponent;

        public void Action(IApplyContext target)
        {
            dynamic documentProvider =
                target.Context.GetComponent<InprocessDocumentComponent>(target.Version)
                      .GetDocumentProvider(target.Version, target.Item.Configuration, target.Item.Metadata,
                                           target.UserName);

            dynamic instanceId = null;
            if (documentProvider != null)
            {
                ApplyTransactionMarker(target);

                _metadataComponent = target.Context.GetComponent<IMetadataComponent>(target.Version);

                IEnumerable<dynamic> result = _metadataComponent.GetMetadataList(target.Version,
                                                                                 target.Item.Configuration,
                                                                                 target.Item.Metadata,
                                                                                 MetadataType.Schema);

                dynamic documentSchema = result.FirstOrDefault();

                bool allowNonSchemaProperties = target.Item.AllowNonSchemaProperties != null &&
                                                target.Item.AllowNonSchemaProperties == true;

                if (target.Item.Document != null)
                {
                    target.Item.Document = AdjustDocumentContent(target.Version, target.Item.Document, documentSchema,
                                                                 allowNonSchemaProperties);

                    target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info(
                        "document \"{0}\" save completed to \"{1}\", type {2}",
                        target.Item.Configuration,
                        target.Item.Document.ToString(),
                        target.Item.Metadata ?? "indexobject");

                    instanceId = target.Item.Document.Id;
                }
                else if (target.Item.Documents != null)
                {
                    var adjustedDocs = new List<dynamic>();

                    foreach (var document in target.Item.Documents)
                    {
                        adjustedDocs.Add(AdjustDocumentContent(target.Version, document, documentSchema,
                                                               allowNonSchemaProperties));

                        target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info(
                            "document \"{0}\" save completed to \"{1}\", type {2}",
                            target.Item.Configuration,
                            document.ToString(),
                            target.Item.Metadata ?? "indexobject");
                    }

                    target.Item.Documents = adjustedDocs.ToArray();

                    target.Context.GetComponent<ILogComponent>(target.Version)
                          .GetLog()
                          .Info("documents save completed to \"{0}\", type {1}", target.Item.Configuration,
                                target.Item.Metadata);
                }
                target.Result = new DynamicWrapper();

                target.Result.IsValid = true;
                target.Result.ValidationMessage = "Document successfully saved.";

                if (instanceId != null)
                {
                    target.Result.Id = instanceId;
                }

                return;
            }
            target.Context.GetComponent<ILogComponent>(target.Version)
                  .GetLog()
                  .Error("document \"{0}\" type \"{1}\" not exists", target.Item.Configuration, target.Item.Metadata);
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
                return null;

            var propertyNamesToRemove = new List<string>();

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
                        ProcessInnerObjectProperties(version, propertyMetadata.TypeInfo, documentProperty);
                    }
                    else if (propertyMetadata.Type == "Array")
                    {
                        if (propertyMetadata.Items == null)
                        {
                            continue;
                        }

                        ProcessInnerArrayProperties(version, propertyMetadata.Items.TypeInfo, documentProperty);
                    }
                }
            }

            foreach (string propertyName in propertyNamesToRemove)
            {
                documentClone[propertyName] = null;
            }

            return documentClone;
        }

        /// <summary>
        ///     Обработка вложенных свойств объектов
        /// </summary>
        private static void ProcessInnerObjectProperties(string version, dynamic propertyTypeInfo,
                                                         dynamic documentProperty)
        {
            if (propertyTypeInfo == null)
            {
                return;
            }

            if (propertyTypeInfo.DocumentLink != null)
            {
                dynamic document = documentProperty.Value;

                if (propertyTypeInfo.DocumentLink.Inline == true)
                {
                    dynamic documentSchema = null;
                    if (propertyTypeInfo.DocumentLink.ConfigId != null &&
                        propertyTypeInfo.DocumentLink.DocumentId != null)
                    {
                        IEnumerable<dynamic> metadataList = _metadataComponent.GetMetadataList(version,
                                                                                               propertyTypeInfo
                                                                                                   .DocumentLink
                                                                                                   .ConfigId,
                                                                                               propertyTypeInfo
                                                                                                   .DocumentLink
                                                                                                   .DocumentId,
                                                                                               MetadataType.Schema);

                        documentSchema = metadataList.FirstOrDefault();
                    }

                    if (documentSchema != null)
                    {
                        RemoveInappropriateProperties(version, document, documentSchema);
                    }
                }
                else
                {
                    if (document != null &&
                        propertyTypeInfo.DocumentLink.ConfigId != null &&
                        propertyTypeInfo.DocumentLink.DocumentId != null)
                    {
                        RemoveLinkExtraProperties(document);
                    }
                }
            }
            else if (propertyTypeInfo.BinaryLink != null)
            {
                // TODO
            }
            else
            {
                RemoveInappropriateProperties(version, documentProperty.Value, propertyTypeInfo);
            }
        }

        /// <summary>
        ///     Обработка вложенных свойств массивов
        /// </summary>
        private static void ProcessInnerArrayProperties(string version, dynamic propertyTypeInfo,
                                                        dynamic documentProperty)
        {
            if (propertyTypeInfo == null)
            {
                return;
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
                                                                                               propertyTypeInfo
                                                                                                   .DocumentLink
                                                                                                   .ConfigId,
                                                                                               propertyTypeInfo
                                                                                                   .DocumentLink
                                                                                                   .DocumentId,
                                                                                               MetadataType.Schema);

                        documentSchema = metadataList.FirstOrDefault();
                    }


                    if (documentSchema != null)
                    {
                        foreach (var documentValue in DynamicWrapperExtensions.ToEnumerable(documentProperty.Value))
                        {
                            RemoveInappropriateProperties(version, documentValue, documentSchema);
                        }
                    }
                }
                else
                {
                    foreach (var documentValue in DynamicWrapperExtensions.ToEnumerable(documentProperty.Value))
                    {
                        RemoveLinkExtraProperties(documentValue);
                    }
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
                    RemoveInappropriateProperties(version, documentValue, propertyTypeInfo);
                }
            }
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

            foreach (string propertyName in propertyNames)
            {
                instanceToHandle[propertyName] = null;
            }
        }

        /// <summary>
        ///     Присоединить сохраняемые документы к указанной транзакции
        /// </summary>
        /// <param name="target"></param>
        private void ApplyTransactionMarker(IApplyContext target)
        {
            if (!string.IsNullOrEmpty(target.TransactionMarker))
            {
                dynamic docs = (target.Item.Document != null ? new[] {target.Item.Document} : null) ??
                               target.Item.Documents;

                if (docs != null)
                {
                    target.Context.GetComponent<ITransactionComponent>(target.Version).GetTransactionManager()
                          .GetTransaction(target.TransactionMarker)
                          .Attach(target.Item.Configuration,
                                  target.Item.Metadata,
                                  target.Version,
                                  docs,
                                  target.Context.GetComponent<ISecurityComponent>(target.Version)
                                        .GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ??
                                  AuthorizationStorageExtensions.AnonimousUser);
                }
            }
        }
    }
}