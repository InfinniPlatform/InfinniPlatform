using System;
using System.Collections.Generic;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetDocumentCrossConfig
    {
        public void Action(IApplyContext target)
        {
            IEnumerable<object> filter = DynamicWrapperExtensions.ToEnumerable(target.Item.Filter);
            IEnumerable<object> sorting = DynamicWrapperExtensions.ToEnumerable(target.Item.Sorting);
            IEnumerable<object> configs = DynamicWrapperExtensions.ToEnumerable(target.Item.Configurations);
            IEnumerable<object> documents = DynamicWrapperExtensions.ToEnumerable(target.Item.Documents);

            var resultDocuments = new List<dynamic>();

            foreach (string config in configs)
            {
                foreach (string document in documents)
                {
                    var executor =
                        new DocumentExecutor(
                            target.Context.GetComponent<IConfigurationMediatorComponent>(),
                            target.Context.GetComponent<IMetadataComponent>(),
                            target.Context.GetComponent<InprocessDocumentComponent>(),
                            target.Context.GetComponent<IProfilerComponent>(),
                            target.Context.GetComponent<ILogComponent>());

                    resultDocuments.AddRange(executor.GetCompleteDocuments(target.Context.GetVersion(config, target.UserName), config, document,
                                                                           target.UserName,
                                                                           Convert.ToInt32(target.Item.PageNumber),
                                                                           Convert.ToInt32(target.Item.PageSize),
                                                                           filter, sorting, target.Item.IgnoreResolve));
                }
            }

            target.Result = resultDocuments;

            target.Context.GetComponent<ILogComponent>()
                  .GetLog()
                  .Info("Cross configuration document search completed");
        }
    }
}