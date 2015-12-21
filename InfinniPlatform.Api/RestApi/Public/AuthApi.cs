using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class AuthApi : IAuthApi
    {
        public AuthApi(Auth.AuthApi authApi)
        {
            _authApi = authApi;
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
    }
}