using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSignIn
    {
        public void Action(IApplyContext target)
        {
            //возвращаем список Cookie, полученных в ходе регистрации
            target.Result = target.Context.GetComponent<SignInApi>(target.Version)
                                  .SignInInternal(target.Item.UserName, target.Item.Password, target.Item.Remember);
        }
    }
}