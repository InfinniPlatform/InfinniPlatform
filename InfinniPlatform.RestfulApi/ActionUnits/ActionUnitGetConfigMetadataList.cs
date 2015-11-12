using System.Collections.Generic;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    /// Получить метаданные загруженной конфигурации сервера
    /// </summary>
    public sealed class ActionUnitGetConfigMetadataList
    {
        public void Action(IApplyContext target)
        {
            var paramsDoc = target.Item;

            if (target.Item.Document != null)
            {
                paramsDoc = target.Item.Document;
            }

            target.Result = target.Context.GetComponent<IMetadataComponent>()
                                  .GetMetadataList(paramsDoc.Configuration, paramsDoc.Metadata, paramsDoc.MetadataType);

            var result = new List<dynamic>();

            foreach (var item in target.Result)
            {
                if (item != null)
                {
                    result.Add(item);
                }
            }

            target.Result = result;
        }
    }
}