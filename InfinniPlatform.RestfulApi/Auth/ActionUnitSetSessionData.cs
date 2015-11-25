using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
	public sealed class ActionUnitSetSessionData
	{
		public void Action(IApplyContext target)
		{
			target.Context.GetComponent<ISessionManager>().SetSessionData(target.Item.ClaimType, target.Item.ClaimValue);
			
			target.Result = new DynamicWrapper();
			target.Result.ValidationMessage = "Session data added successfully";
			target.Result.IsValid = true;
		}
	}
}