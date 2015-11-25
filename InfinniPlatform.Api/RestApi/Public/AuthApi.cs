using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class AuthApi : IAuthApi
    {
        public dynamic AddUser(string userName, string password)
        {
            return new Auth.AuthApi().AddUser(userName, password);
        }

        public dynamic DeleteUser(string userName)
        {
            return new Auth.AuthApi().RemoveUser(userName);
        }

        public dynamic GetUser(string userName)
        {
            return new Auth.AuthApi().GetUser(userName);
        }

        public dynamic GrantAccess(string userName, string application, string documentType = null, string service = null,
            string instanceId = null)
        {
            return new Auth.AuthApi().GrantAccess(userName, application, documentType, service, instanceId);
        }

        public dynamic DenyAccess(string userName, string application, string documentType = null, string service = null,
            string instanceId = null)
        {
            return new Auth.AuthApi().DenyAccess(userName, application, documentType, service, instanceId);
        }

        public dynamic SetSessionData(string userName, string claimType, string claimValue)
        {
            return new Auth.AuthApi().SetSessionData(userName, claimType, claimValue);
        }

        public dynamic GetSessionData(string userName, string claimType)
        {
            return new Auth.AuthApi().GetSessionData(userName, claimType);
        }

        public dynamic RemoveSessionData(string userName, string claimType)
        {
            return new Auth.AuthApi().RemoveSessionData(userName, claimType);
        }

        public dynamic AddRole(string roleName)
        {
            return new Auth.AuthApi().AddRole(roleName, roleName, roleName);
        }

        public dynamic DeleteRole(string roleName)
        {
            return new Auth.AuthApi().RemoveRole(roleName);
        }

        public dynamic AddUserRole(string userName, string roleName)
        {
            return new Auth.AuthApi().RemoveUserRole(userName, roleName);
        }

        public dynamic DeleteUserRole(string userName, string roleName)
        {
            return new Auth.AuthApi().RemoveUserRole(userName, roleName);
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
