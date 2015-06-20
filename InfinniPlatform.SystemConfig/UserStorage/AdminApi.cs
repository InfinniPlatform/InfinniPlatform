using System;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.SystemConfig.UserStorage
{
    public sealed class AdminApi
    {
        private void TryApplyAccess(Func<dynamic> applyAccess)
        {
            dynamic result = ((object) applyAccess.Invoke()).ToDynamic();
            if (result.IsValid != null && result.IsValid == false)
            {
                throw new ArgumentException(string.Format(Resources.FailToSetUserAccess, result.ValidationMessage));
            }
        }

        /// <summary>
        ///     Добавить права пользователю на чтение пользователей
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public void AddAnonimousUserAcl(string userName)
        {
            var aclApi = new AuthApi(null);

            TryApplyAccess(() => aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId,
                                                    AuthorizationStorageExtensions.AclStore, "getdocument"));
            TryApplyAccess(() => aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId,
                                                    AuthorizationStorageExtensions.RoleStore, "getdocument"));
            TryApplyAccess(() => aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId,
                                                    AuthorizationStorageExtensions.UserRoleStore, "getdocument"));
            TryApplyAccess(() => aclApi.GrantAccess(userName, AuthorizationStorageExtensions.AuthorizationConfigId,
                                                    AuthorizationStorageExtensions.UserStore, "getdocument"));
        }


        public dynamic GrantAdminAcl(string userName)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "grantadminacl", null, new
                {
                    UserName = userName
                }).ToDynamic();
        }

        public dynamic SetDefaultAcl()
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "setdefaultacl", null, null).ToDynamic();
        }
    }
}