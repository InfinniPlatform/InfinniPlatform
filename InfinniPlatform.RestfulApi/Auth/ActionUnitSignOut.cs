using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSignOut
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<SignInApi>(target.Version).SignOutInternal();
        }
    }
}