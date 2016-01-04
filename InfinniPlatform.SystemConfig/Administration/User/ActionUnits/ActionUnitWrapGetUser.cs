using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    /// <summary>
    ///     Получить пользователя системы
    /// </summary>
    public sealed class ActionUnitWrapGetUser
    {
        public void Action(IApplyContext target)
        {
            var aclApi = target.Context.GetComponent<AuthApi>();

            dynamic user = null;

            user = target.Item.Document ?? target.Item;

            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                target.IsValid = false;
                target.ValidationMessage = "User name is not specified";
                return;
            }

            var userFound =
                aclApi.GetUsers(false)
                    .FirstOrDefault(r => r.UserName.ToLowerInvariant() == user.UserName.ToLowerInvariant());


            if (userFound == null)
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = "User with user name " + user.UserName + " not found.";
                return;
            }
            target.Result = userFound;
        }
    }
}