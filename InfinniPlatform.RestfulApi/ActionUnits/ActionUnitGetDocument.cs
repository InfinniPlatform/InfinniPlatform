using System;

using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitGetDocument
	{
		public void Action(IApplyContext target)
		{
			var executor =
				new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(),
					target.Context.GetComponent<IMetadataComponent>(),
					target.Context.GetComponent<InprocessDocumentComponent>(),
					target.Context.GetComponent<IProfilerComponent>(),
					target.Context.GetComponent<ILogComponent>());

			target.Result = executor.GetCompleteDocuments(
				null,
				target.Item.Configuration,
				target.Item.Metadata,
				target.UserName,
				Convert.ToInt32(target.Item.PageNumber),
				Convert.ToInt32(target.Item.PageSize),
				target.Item.Filter,
				target.Item.Sorting,
				target.Item.IgnoreResolve);
		}
	}
}