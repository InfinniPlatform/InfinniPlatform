using System.Collections.Generic;

using InfinniPlatform.Auth.HttpService.Controllers;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// User information available through <see cref="AuthInternalController{TUser}" />.
    /// </summary>
    internal class PublicUserInfo
    {
        public PublicUserInfo(string userName,
                              string displayName,
                              string description,
                              IEnumerable<string> roles,
                              IEnumerable<AppUserLogin> logins,
                              List<AppUserClaim> claims)
        {
            UserName = userName;
            DisplayName = displayName;
            Description = description;
            Roles = roles;
            Logins = logins;
            Claims = claims;
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<AppUserLogin> Logins { get; set; }

        public List<AppUserClaim> Claims { get; set; }
    }
}