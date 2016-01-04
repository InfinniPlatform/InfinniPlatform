using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Auth
{
    /// <summary>
    /// Модуль для получения утверждений относительно пользователя
    /// </summary>
    public sealed class ActionUnitGetSessionData
    {
        public ActionUnitGetSessionData(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        public void Action(IApplyContext target)
        {
            var tenantId = _sessionManager.GetSessionData(target.Item.ClaimType);

            target.Result = new DynamicWrapper();
            target.Result.ClaimValue = tenantId;
        }
    }
}