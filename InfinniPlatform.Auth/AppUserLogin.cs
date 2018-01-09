using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Application user login representation.
    /// </summary>
    public class AppUserLogin
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AppUserLogin" />.
        /// </summary>
        public AppUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            LoginProvider = loginProvider;
            ProviderDisplayName = providerDisplayName;
            ProviderKey = providerKey;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AppUserLogin" />.
        /// </summary>
        public AppUserLogin(UserLoginInfo login)
        {
            LoginProvider = login.LoginProvider;
            ProviderDisplayName = login.ProviderDisplayName;
            ProviderKey = login.ProviderKey;
        }

        /// <summary>
        /// Login provider name.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Login provider display name.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Login provider key.
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Create new instance of <see cref="UserLoginInfo"/> from this <see cref="AppUserLogin"/> instance.
        /// </summary>
        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }
    }
}