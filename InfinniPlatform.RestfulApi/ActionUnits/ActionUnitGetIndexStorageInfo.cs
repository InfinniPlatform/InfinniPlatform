using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.RestfulApi.Extensions;

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
