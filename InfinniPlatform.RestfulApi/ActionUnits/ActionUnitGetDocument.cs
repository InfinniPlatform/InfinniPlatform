using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
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

            var executor = new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(target.Version),
                                                target.Context.GetComponent<IMetadataComponent>(target.Version),
                                                target.Context.GetComponent<InprocessDocumentComponent>(target.Version),
                                                target.Context.GetComponent<IProfilerComponent>(target.Version));
		    
			target.Result = executor.GetCompleteDocuments(target.Version, target.Item.Configuration, target.Item.Metadata, target.UserName,
			                                              Convert.ToInt32(target.Item.PageNumber),
			                                              Convert.ToInt32(target.Item.PageSize),
			                                              target.Item.Filter, target.Item.Sorting, target.Item.IgnoreResolve);
		}
	}
}
