using System;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для MetadataApi.
    /// </summary>
    internal sealed class MetadataApiHttpService : IHttpService
    {
        public MetadataApiHttpService(IMetadataComponent metadataComponent)
        {
            _metadataComponent = metadataComponent;
        }

        private readonly IMetadataComponent _metadataComponent;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/SystemConfig/StandardApi/metadata";

            builder.Post["/GetManagedMetadata"] = CreateHandler(GetMetadataItem);
        }

        private static Func<IHttpRequest, object> CreateHandler(Action<IActionContext> action)
        {
            return new ChangeHttpRequestHandler(action).Action;
        }

        private void GetMetadataItem(IActionContext target)
        {
            string configuration = target.Item.Configuration;

            Func<dynamic, bool> predicate;

            // если не указано конкретное наименование метаданных (наименование конкретной формы, например "SomeForm1", то ищем первую попавшуюся форму указанного типа MetadataType)
            if (string.IsNullOrEmpty(target.Item.MetadataName))
            {
                // если не указан тип и наименование, берем первое попавшееся view
                if (string.IsNullOrEmpty(target.Item.MetadataType))
                {
                    predicate = view => true;
                }
                else
                {
                    predicate = view => view.MetadataType == target.Item.MetadataType;
                }
            }
            // иначе ищем метаданные с указанным наименованием MetadataName и указанным типом MetadataType
            else
            {
                predicate = view => view.Name == target.Item.MetadataName;
            }

            var itemMetadata = _metadataComponent.GetMetadataItem(configuration, target.Item.MetadataObject, MetadataType.View, predicate);

            // если нашли существующие метаданные - возвращаем их
            if (itemMetadata != null)
            {
                target.Result = itemMetadata;
                target.Result.RequestParameters = target.Item.Parameters;
            }
        }
    }
}