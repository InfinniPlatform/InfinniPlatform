using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using System;
using System.Collections.Generic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;

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

			List<dynamic> resultDocuments = new List<dynamic>();

	        foreach (string config in configs)
	        {
		        foreach (string document in documents)
		        {
                    var executor = new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(target.Version),
                                                        target.Context.GetComponent<IMetadataComponent>(target.Version),
                                                        target.Context.GetComponent<InprocessDocumentComponent>(target.Version),
                                                        target.Context.GetComponent<IProfilerComponent>(target.Version));

					resultDocuments.AddRange(executor.GetCompleteDocuments(target.Version, config, document, target.UserName,
										  Convert.ToInt32(target.Item.PageNumber), Convert.ToInt32(target.Item.PageSize),
										  filter, sorting, target.Item.IgnoreResolve));
		        }
	        }

	        target.Result = resultDocuments;

            target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info("Cross configuration document search completed");
        }
    }
}
