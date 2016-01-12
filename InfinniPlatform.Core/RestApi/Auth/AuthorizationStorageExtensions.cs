namespace InfinniPlatform.Core.RestApi.Auth
{
    public static class AuthorizationStorageExtensions
    {
        public const string AuthorizationConfigId = "Authorization";
        public const string AdministrationConfigId = "Administration";
        public const string UserStore = "UserStore";
        public const string UserRoleStore = "UserRoleStore";
        public const string RoleStore = "RoleStore";
        public const string ClaimStore = "ClaimStore";
        public const string AdminRole = "System administrator";
        public const string Default = "Default";
        public const string AnonimousUserCredentials = "AnonimousCredentials";
        public const string CustomCredentials = "CustomCredentials";

        public const string SystemTenant = "system";
        public const string AnonymousUser = "anonimous";
        public const string UnknownUser = "unknown";

        public const string TenantId = "tenantid";
        public const string DefaultTenantId = "defaulttenantid";
    }
}