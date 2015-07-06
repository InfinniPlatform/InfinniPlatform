using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitChangePassword
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<SignInApi>()
                .ChangePassword(target.Item.Document.UserName, target.Item.Document.OldPassword,
                    target.Item.Document.NewPassword);
            target.Result = new DynamicWrapper();
        }
    }
}