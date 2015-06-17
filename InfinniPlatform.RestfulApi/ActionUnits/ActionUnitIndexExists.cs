using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.RestfulApi.Extensions;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitIndexExists
	{
		public void Action(IApplyResultContext target)
		{
			dynamic result = new DynamicWrapper();
			result.IndexExists = IndexedStorageExtension.IndexExists(target.Item.Configuration,target.Item.Metadata ?? string.Empty);
			result.IsValid = true;
			result.ValidationMessage = "Index status successfully checked";
			target.Result = result;

            target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info("metadata index \"{0}\" status checked for configuration \"{1}\" ", target.Item.Metadata, target.Item.Configuration);
		}
	}
}
