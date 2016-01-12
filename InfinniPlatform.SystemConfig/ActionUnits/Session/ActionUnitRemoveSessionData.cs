using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.SystemConfig.ActionUnits.Session
{
    /// <summary>
    /// Удаление утверждения относительно пользователя (Claim).
    /// </summary>
    public sealed class ActionUnitRemoveSessionData
    {
        public ActionUnitRemoveSessionData(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        public void Action(IApplyContext target)
        {
            _sessionManager.SetSessionData(target.Item.ClaimType, null);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
        }
    }
}