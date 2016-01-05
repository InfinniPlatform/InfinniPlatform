using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Credentials
{
    public sealed class ActionUnitSetAnonimousCredentials
    {
        public void Action(IApplyContext target)
        {
            target.UserName = AuthorizationStorageExtensions.AnonymousUser;
        }
    }
}