using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitRestIsReady
	{
		public void Action(IApplyResultContext target)
		{
			target.Result = new DynamicWrapper();
			target.Result.Status = "200";
		}
	}
}
