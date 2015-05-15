using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Index;
using InfinniPlatform.Metadata;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitGetDocument
	{
		public void Action(IApplyContext target)
		{

			var executor = new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(),
			                                    target.Context.GetComponent<IMetadataComponent>(),
			                                    target.Context.GetComponent<InprocessDocumentComponent>(),
			                                    target.Context.GetComponent<IProfilerComponent>());

			target.Result = executor.GetCompleteDocuments(target.Item.Configuration, target.Item.Metadata, target.UserName,
			                                              Convert.ToInt32(target.Item.PageNumber),
			                                              Convert.ToInt32(target.Item.PageSize),
			                                              target.Item.Filter, target.Item.Sorting, target.Item.IgnoreResolve);
		}
	}
}
