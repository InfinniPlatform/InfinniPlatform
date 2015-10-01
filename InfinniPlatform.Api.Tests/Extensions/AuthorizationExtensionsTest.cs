using System.Collections.Generic;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.Api.Tests.Extensions
{
    public static class AuthorizationExtensionsTest
    {
        public static void ClearAuthConfig()
        {
            new SignInApi().SignInInternal("Admin", "Admin", false);

            var aclApi = new AuthApi();

            IEnumerable<dynamic> userRoles = aclApi.GetUserRoles();
            foreach (dynamic userRole in userRoles)
            {
                if (userRole.RoleName != AuthorizationStorageExtensions.AdminRole &&
                    userRole.RoleName != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveUserRole(userRole.UserName, userRole.RoleName);
                }
            }

            IEnumerable<dynamic> users = aclApi.GetUsers();
            foreach (dynamic user in users)
            {
                if (user.UserName != AuthorizationStorageExtensions.AdminUser &&
                    user.UserName != AuthorizationStorageExtensions.AnonimousUser &&
                    user.UserName != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveUser(user.UserName);
                }
            }

            IEnumerable<dynamic> roles = aclApi.GetRoles();
            foreach (dynamic role in roles)
            {
                if (role.Name != AuthorizationStorageExtensions.AdminRole &&
                    role.Name != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveRole(role.Name);
                }
            }

            IEnumerable<dynamic> acl = aclApi.GetAcl();
            foreach (dynamic a in acl)
            {
                if (a.UserName != AuthorizationStorageExtensions.AdminRole &&
                    a.UserName != AuthorizationStorageExtensions.AdminUser &&
                    a.UserName != AuthorizationStorageExtensions.AnonimousUser)
                {
                    aclApi.RemoveAcl(a.Id);
                }
            }

            new SignInApi().SignOutInternal();
        }
    }
}