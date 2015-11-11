using System.Collections.Generic;

using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetFromIndex
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = IndexedStorageExtension.GetDocument(target.Item.Id, target.Item.Configuration,
                target.Item.Metadata ?? string.Empty);


            var log = target.Context.GetComponent<ILogComponent>().GetLog();

            if (target.Result != null)
            {
                log.Info("Found document from index.", new Dictionary<string, object>
                                                       {
                                                           { "result", target.Result.ToString() },
                                                           { "index", target.Item.Configuration },
                                                           { "type", target.Item.Metadata },
                                                       });
            }
            else
            {
                log.Error("No documents found from type.", new Dictionary<string, object>
                                                           {
                                                               { "type", target.Item.Metadata },
                                                           });
            }
        }
    }
}