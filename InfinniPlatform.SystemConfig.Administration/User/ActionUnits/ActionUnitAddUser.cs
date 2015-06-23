using System.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitAddUser
    {
        public void Action(IApplyContext target)
        {
            var aclApi = target.Context.GetComponent<AuthApi>(target.Version);

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

            if (userFound != null && user.Password != null)
            {
                target.IsValid = false;
                target.ValidationMessage = "User with user name " + user.UserName + " already exists.";
                return;
            }

            if (user.Password != null)
            {
                aclApi.AddUser(user.UserName, user.Password);
            }

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    aclApi.AddUserToRole(user.UserName, userRole.DisplayName);
                }
            }


            if (user.IsAdmin == true)
            {
                aclApi.AddUserToRole(user.UserName, AuthorizationStorageExtensions.AdminRole);
            }

            user.Password = null;

            target.Context.GetComponent<DocumentApi>(target.Version)
                .SetDocument(AuthorizationStorageExtensions.AdministrationConfigId, "User",
                    user);

            RestQueryApi.QueryPostJsonRaw("AdministrationCustomization", "Common", "OnAddUserEvent", null, user,
                target.Version);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = Resources.UserCreatedSuccessfully;
        }
    }
}