using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetIndexStorageInfo
	{
		public void Action(IApplyResultContext target)
		{
		    target.Result = IndexedStorageExtension.GetStatus();
		}
	}
}
