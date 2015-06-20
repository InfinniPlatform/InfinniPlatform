using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetDocument
    {
        public void Action(IApplyContext target)
        {
            var executor =
                new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(target.Version),
                                     target.Context.GetComponent<IMetadataComponent>(target.Version),
                                     target.Context.GetComponent<InprocessDocumentComponent>(target.Version),
                                     target.Context.GetComponent<IProfilerComponent>(target.Version));

            target.Result = executor.GetCompleteDocuments(target.Version, target.Item.Configuration,
                                                          target.Item.Metadata, target.UserName,
                                                          Convert.ToInt32(target.Item.PageNumber),
                                                          Convert.ToInt32(target.Item.PageSize),
                                                          target.Item.Filter, target.Item.Sorting,
                                                          target.Item.IgnoreResolve);
        }
    }
}