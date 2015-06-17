using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.Api.Tests.Extensions
{
    public static class AuthorizationExtensionsTest
    {
        public static void ClearAuthConfig()
        {
            new SignInApi(null).SignInInternal("Admin", "Admin", false);

            var aclApi = new AuthApi(null);

            var userRoles = aclApi.GetUserRoles();
            foreach (var userRole in userRoles)
            {
                if (userRole.RoleName != AuthorizationStorageExtensions.AdminRole &&
                    userRole.RoleName != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveUserRole(userRole.UserName, userRole.RoleName);
                }
            }

            var users = aclApi.GetUsers();
            foreach (var user in users)
            {
                if (user.UserName != AuthorizationStorageExtensions.AdminUser &&
                    user.UserName != AuthorizationStorageExtensions.AnonimousUser &&
                    user.UserName != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveUser(user.UserName);
                }
            }

            var roles = aclApi.GetRoles();
            foreach (var role in roles)
            {
                if (role.Name != AuthorizationStorageExtensions.AdminRole &&
                    role.Name != AuthorizationStorageExtensions.Default)
                {
                    aclApi.RemoveRole(role.Name);
                }
            }

            var acl = aclApi.GetAcl();
            foreach (var a in acl)
            {
                if (a.UserName != AuthorizationStorageExtensions.AdminRole &&
                    a.UserName != AuthorizationStorageExtensions.AdminUser &&
                    a.UserName != AuthorizationStorageExtensions.AnonimousUser)
                {
                    aclApi.RemoveAcl(a.Id);
                }
            }

            new SignInApi(null).SignOutInternal();

        }

    }
}
