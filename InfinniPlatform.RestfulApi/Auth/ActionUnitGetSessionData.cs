using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	/// Модуль для получения утверждений относительно пользователя
	/// </summary>
	public sealed class ActionUnitGetSessionData
	{
		public void Action(IApplyContext target)
		{
			var tenantId = target.Context.GetComponent<ISessionManager>().GetSessionData(target.Item.ClaimType);

			target.Result = new DynamicWrapper();
			target.Result.ClaimValue = tenantId;
		}
	}
}