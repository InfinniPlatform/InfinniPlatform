using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Удаление утверждения относительно пользователя (Claim).
    /// </summary>
    public sealed class ActionUnitRemoveSessionData
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<ISessionManager>().SetSessionData(target.Item.ClaimType, null);

			target.Result = new DynamicWrapper();
            target.Result.ValidationMessage = "Session data removed successfully";
            target.Result.IsValid = true;
        }
    }
}