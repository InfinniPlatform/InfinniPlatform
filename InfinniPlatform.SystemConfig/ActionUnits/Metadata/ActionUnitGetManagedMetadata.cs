using System;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.ActionUnits.Metadata
{
    /// <summary>
    /// Получение метаданных на высоком уровне абстракции (без указания запроса IQL)
    /// </summary>
    public class ActionUnitGetManagedMetadata
    {
        public ActionUnitGetManagedMetadata(IMetadataComponent metadataComponent)
        {
            _metadataComponent = metadataComponent;
        }

        private readonly IMetadataComponent _metadataComponent;

        public void Action(IApplyContext target)
        {
            Func<dynamic, bool> predicate;

            //если не указано конкретное наименование метаданных (наименование конкретной формы, например "SomeForm1", то ищем первую попавшуюся форму указанного типа MetadataType)
            if (string.IsNullOrEmpty(target.Item.MetadataName))
            {
                //если не указан тип и наименование, берем первое попавшееся view
                if (string.IsNullOrEmpty(target.Item.MetadataType))
                {
                    predicate = view => true;
                }
                else
                {
                    predicate = view => view.MetadataType == target.Item.MetadataType;
                }
            }
            //иначе ищем метаданные с указанным наименованием MetadataName и указанным типом MetadataType
            else
            {
                predicate = view => view.Name == target.Item.MetadataName;
            }

            var itemMetadata = _metadataComponent.GetMetadataItem(target.Item.Configuration, target.Item.MetadataObject, MetadataType.View, predicate);

            //если нашли существующие метаданные - возвращаем их
            if (itemMetadata != null)
            {
                target.Result = itemMetadata;
                target.Result.RequestParameters = target.Item.Parameters;
            }
        }
    }
}