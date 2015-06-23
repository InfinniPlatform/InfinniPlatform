using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

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