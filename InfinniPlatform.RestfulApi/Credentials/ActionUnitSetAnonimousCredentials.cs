using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Credentials
{
    public sealed class ActionUnitSetAnonimousCredentials
    {
        public void Action(IApplyContext target)
        {
            target.UserName = AuthorizationStorageExtensions.AnonimousUser;
        }
    }
}