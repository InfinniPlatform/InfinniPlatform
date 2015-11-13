using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Credentials
{
    public sealed class ActionUnitSetAnonimousCredentials
    {
        public void Action(IApplyContext target)
        {
            target.UserName = AuthorizationStorageExtensions.AnonymousUser;
        }
    }
}