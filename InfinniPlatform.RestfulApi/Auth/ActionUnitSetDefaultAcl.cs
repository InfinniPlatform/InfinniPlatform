using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSetDefaultAcl
    {
        public ActionUnitSetDefaultAcl(AuthApi authApi)
        {
            _authApi = authApi;
        }

        private readonly AuthApi _authApi;

        public void Action(IApplyContext target)
        {
            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "User", "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "User", "setdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Organization", "setdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Organization", "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "Menu", "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "MenuItemSettings",
                "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "MenuPermission", "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                "setdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AdministrationConfigId, "UserOrganizationSession",
                "deletedocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                AuthorizationStorageExtensions.AclStore, "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                AuthorizationStorageExtensions.RoleStore, "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                AuthorizationStorageExtensions.UserRoleStore, "getdocument");

            _authApi.GrantAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId,
                AuthorizationStorageExtensions.UserStore, "getdocument");

            _authApi.DenyAccessAll(AuthorizationStorageExtensions.AdministrationConfigId);

            _authApi.DenyAccessAll(AuthorizationStorageExtensions.AuthorizationConfigId);
        }
    }
}