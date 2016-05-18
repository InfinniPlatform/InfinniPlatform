using System.Collections.Generic;

using InfinniPlatform.Core.Security;

namespace InfinniPlatform.Authentication.Services
{
    public class SafeUserInfo
    {
        public SafeUserInfo(string userName,
                            string displayName,
                            string description,
                            IEnumerable<ForeignKey> roles,
                            IEnumerable<ApplicationUserLogin> logins,
                            List<ApplicationUserClaim> claims)
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

        public IEnumerable<ForeignKey> Roles { get; set; }

        public IEnumerable<ApplicationUserLogin> Logins { get; set; }

        public List<ApplicationUserClaim> Claims { get; set; }
    }
}