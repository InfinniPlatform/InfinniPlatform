using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class SignInApi : ISignInApi
    {
        public dynamic SignInInternal(string userName, string password, bool remember)
        {
            return new Auth.SignInApi().SignInInternal(userName, password, remember);
        }

        public dynamic ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return new Auth.SignInApi().ChangePassword(userName, oldPassword, newPassword);
        }

        public dynamic SignOut()
        {
            return new Auth.SignInApi().SignOutInternal();
        }
    }
}
