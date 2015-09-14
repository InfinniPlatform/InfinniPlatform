using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSetDefaultAcl
    {
        public void Action(IApplyContext target)
        {
            var aclApi = new AuthApi();

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "User", "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "User", "setdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Organization", "setdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Organization", "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Menu", "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "MenuItemSettings",
                                  "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "MenuPermission", "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                                  "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                                  "setdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                                  "deletedocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                                  AuthorizationStorageExtensions.AclStore, "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                                  AuthorizationStorageExtensions.RoleStore, "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                                  AuthorizationStorageExtensions.UserRoleStore, "getdocument");

            aclApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                                  AuthorizationStorageExtensions.UserStore, "getdocument");

            aclApi.DenyAccessAll(AuthorizationStorageExtensions.AdministrationConfigId);

            aclApi.DenyAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId);
        }
    }
}