using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSetSessionData
    {
        public ActionUnitSetSessionData(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        public void Action(IApplyContext target)
        {
            _sessionManager.SetSessionData(target.Item.ClaimType, target.Item.ClaimValue);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
        }
    }
}