using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    /// <summary>
    ///     Добавление новой роли в конфигурации администрирования
    /// </summary>
    public sealed class ActionUnitWrapAddRole
    {
        public void Action(IApplyContext target)
        {
            var aclApi = target.Context.GetComponent<AuthApi>();

            var role = target.Item.Document ?? target.Item;

            target.Result = new DynamicWrapper();

            if (role == null || string.IsNullOrEmpty(role.Name))
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = "Role name is not specified";
                return;
            }

            var roleFound =
                aclApi.GetRoles().FirstOrDefault(r => r.Name.ToLowerInvariant() == role.Name.ToLowerInvariant());

            if (roleFound != null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = "Role with name " + role.Name + " already exists.";
                return;
            }

            aclApi.AddRole(role.Name, role.Name, role.Name);

            target.Context.GetComponent<DocumentApi>()
                .SetDocument(AuthorizationStorageExtensions.AdministrationConfigId, "Role",
                    role);


            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role added.";
        }
    }
}