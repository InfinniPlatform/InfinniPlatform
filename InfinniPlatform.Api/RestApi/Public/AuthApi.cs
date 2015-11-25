using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class AuthApi : IAuthApi
    {
        public AuthApi()
        {
            _authApi = new Auth.AuthApi();
        }


        private readonly Auth.AuthApi _authApi;


        public dynamic AddUser(string userName, string password)
        {
            return _authApi.AddUser(userName, password);
        }

        public dynamic DeleteUser(string userName)
        {
            return _authApi.RemoveUser(userName);
        }

        public dynamic GetUser(string userName)
        {
            return _authApi.GetUser(userName);
        }

        public dynamic GrantAccess(string userName, string application, string documentType = null, string service = null, string instanceId = null)
        {
            return _authApi.GrantAccess(userName, application, documentType, service, instanceId);
        }

        public dynamic DenyAccess(string userName, string application, string documentType = null, string service = null,
                                  string instanceId = null)
        {
            return _authApi.DenyAccess(userName, application, documentType, service, instanceId);
        }

        public dynamic SetSessionData(string userName, string claimType, string claimValue)
        {
            return _authApi.SetSessionData(userName, claimType, claimValue);
        }

        public dynamic GetSessionData(string userName, string claimType)
        {
            return _authApi.GetSessionData(userName, claimType);
        }

        public dynamic RemoveSessionData(string userName, string claimType)
        {
            return _authApi.RemoveSessionData(userName, claimType);
        }

        public dynamic AddRole(string roleName)
        {
            return _authApi.AddRole(roleName, roleName, roleName);
        }

        public dynamic DeleteRole(string roleName)
        {
            return _authApi.RemoveRole(roleName);
        }

        public dynamic AddUserRole(string userName, string roleName)
        {
            return _authApi.RemoveUserRole(userName, roleName);
        }

        public dynamic DeleteUserRole(string userName, string roleName)
        {
            return _authApi.RemoveUserRole(userName, roleName);
        }

        public IEnumerable<object> GetAclList(AclType aclType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            var docApi = new DocumentApi();

            if (aclType == AclType.User)
            {
                return docApi.GetDocument("Administration", "User", filter, pageNumber, pageSize, sorting);
            }

            if (aclType == AclType.Role)
            {
                return docApi.GetDocument("Administration", "Role", filter, pageNumber, pageSize, sorting);
            }

            throw new ArgumentException(string.Format("Cant get acl for type {0}", aclType));
        }
    }
}
