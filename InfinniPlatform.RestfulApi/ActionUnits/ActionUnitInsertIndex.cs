using System.Collections.Generic;

using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitInsertIndex
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.SetDocument(target.Item.Item, target.Item.Configuration,
                                                target.Item.Metadata ?? string.Empty);

            target.Context.GetComponent<ILogComponent>().GetLog().Info(
                "Document inserted.", new Dictionary<string, object>
                                                                                   {
                                                                                       { "document", target.Item.ToString() },
                                                                                       { "configurationId", target.Item.Configuration },
                                                                                       { "type", target.Item.Metadata },
                                                                                   });
        }
    }
}