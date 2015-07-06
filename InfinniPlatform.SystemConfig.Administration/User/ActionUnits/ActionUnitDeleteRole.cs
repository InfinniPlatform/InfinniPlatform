using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitDeleteRole
    {
        public void Action(IApplyContext target)
        {
            var aclApi = target.Context.GetComponent<AuthApi>();

            var role = target.Item.Document ?? target.Item;

            var roleFound =
                aclApi.GetRoles().FirstOrDefault(r => r.Name.ToLowerInvariant() == role.RoleName.ToLowerInvariant());

            if (roleFound == null)
            {
                target.IsValid = false;
                target.ValidationMessage = "Role with name " + role.RoleName + " not found.";
                return;
            }

            aclApi.RemoveRole(role.RoleName);

            var api = target.Context.GetComponent<DocumentApi>();
            dynamic extendedRole = api.GetDocument(AuthorizationStorageExtensions.AdministrationConfigId, "role",
                f => f.AddCriteria(cr => cr.Property("Name").IsEquals(roleFound.Name)), 0, 1).FirstOrDefault();

            if (extendedRole != null)
            {
                api.DeleteDocument(AuthorizationStorageExtensions.AdministrationConfigId, "role", extendedRole.Id);
            }

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role with name " + role.RoleName + " deleted.";
        }
    }
}