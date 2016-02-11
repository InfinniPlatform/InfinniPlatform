using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Utils
{
    public sealed class MetadataOperator
    {
        private readonly DocumentLinkMap _linkMap;
        private readonly IMetadataApi _metadataComponent;
        private readonly List<dynamic> _typeInfoChain;

        public MetadataOperator(IMetadataApi metadataComponent,
                                DocumentLinkMap linkMap,
                                IEnumerable<dynamic> typeInfoChain = null)
        {
            _metadataComponent = metadataComponent;
            _linkMap = linkMap;
            _typeInfoChain = typeInfoChain != null
                                 ? typeInfoChain.ToList()
                                 : new List<dynamic>();
        }

        public IEnumerable<dynamic> TypeInfoChain => _typeInfoChain;

        public void ProcessMetadata(dynamic document, dynamic typeInfo)
        {
            if (document == null)
            {
                throw new ArgumentException("document should not be null");
            }


            if (typeInfo == null || string.IsNullOrEmpty(typeInfo.DocumentId))
            {
                return;
            }

            dynamic schema = _metadataComponent.GetDocumentSchema(typeInfo.DocumentId);

            //обрабатываем связанные документы
            if (schema != null && schema.Properties != null)
            {
                dynamic properties = schema.Properties;

                foreach (dynamic property in properties)
                {
                    if (property.Value.Type != null &&
                        property.Value.Type.ToLowerInvariant() == "object" &&
                        property.Value.TypeInfo != null &&
                        property.Value.TypeInfo.DocumentLink != null)
                    {
                        dynamic documentLink = document[property.Key];

                        if (property.Value.TypeInfo.DocumentLink.Resolve == true && documentLink != null)
                        {
                            dynamic property1 = property;
                            Action<object> setValueAction = value => document[property1.Key] = value;

                            _linkMap.RegisterLink(property.Value.TypeInfo.DocumentLink.DocumentId, documentLink.Id, setValueAction);
                        }

                        if (TypeInfoChain.Any(t => t.DocumentId == property.Value.TypeInfo.DocumentLink.DocumentId))
                        {
                            continue;
                        }

                        _typeInfoChain.Add(property.Value.TypeInfo.DocumentLink);

                        if (property.Value.TypeInfo.DocumentLink.Inline == true && documentLink != null)
                        {
                            ProcessMetadata(documentLink, property.Value.TypeInfo.DocumentLink);
                        }
                    }

                    if (property.Value.Type != null &&
                        property.Value.Type.ToLowerInvariant() == "array" &&
                        property.Value.Items != null &&
                        property.Value.Items.TypeInfo != null &&
                        property.Value.Items.TypeInfo.DocumentLink != null)
                    {
                        dynamic documentLinks = document[property.Key];

                        if (documentLinks != null)
                        {
                            if (property.Value.Items.TypeInfo.DocumentLink.Resolve == true)
                            {
                                foreach (dynamic documentLink in documentLinks)
                                {
                                    dynamic link = documentLink;

                                    //выполняется при разрешении ссылок на документ.
                                    //в случае успешного разрешения необходимо удалить объекты с разрешенной ссылкой из списка
                                    Action<object> setValueAction = value =>
                                        {
                                            dynamic arrayToAddResolvedDocument = document[property.Key];
                                            dynamic propValue = value;

                                            propValue.DisplayName = link.DisplayName;

                                            dynamic removeValue =
                                                ((IList<dynamic>) arrayToAddResolvedDocument).FirstOrDefault(
                                                    r => r.Id == propValue.Id);
                                            if (removeValue != null)
                                            {
                                                ObjectHelper.RemoveItem(arrayToAddResolvedDocument, removeValue);
                                            }

                                            arrayToAddResolvedDocument.Add(value);
                                        };

                                    _linkMap.RegisterLink(property.Value.Items.TypeInfo.DocumentLink.DocumentId,
                                        documentLink != null ? documentLink.Id : null,
                                        setValueAction);
                                }

                                //document[property.Key] = null;
                            }

                            if (property.Value.Items.TypeInfo.DocumentLink.Inline == true)
                            {
                                foreach (dynamic documentLink in documentLinks)
                                {
                                    if (TypeInfoChain.Any(t => t.DocumentId == property.Value.Items.TypeInfo.DocumentLink.DocumentId))
                                    {
                                        continue;
                                    }

                                    _typeInfoChain.Add(property.Value.Items.TypeInfo.DocumentLink);


                                    ProcessMetadata(documentLink, property.Value.Items.TypeInfo.DocumentLink);

                                    foreach (dynamic innerProperty in property.Value.Items.Properties)
                                    {
                                        if (innerProperty.Value.TypeInfo != null &&
                                            innerProperty.Value.TypeInfo.DocumentLink != null)
                                        {
                                            Action<object> setValueAction = (value) =>
                                                {
                                                    dynamic propValue = value;
                                                    propValue.DisplayName = documentLink.DisplayName;

                                                    documentLink[innerProperty.Key] = value;
                                                };


                                            _linkMap.RegisterLink(innerProperty.Value.TypeInfo.DocumentLink.DocumentId,
                                                documentLink != null && documentLink[innerProperty.Key] != null
                                                    ? documentLink.GetProperty(innerProperty.Key).Id
                                                    : null,
                                                setValueAction);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}