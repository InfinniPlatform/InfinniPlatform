using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Настройки провайдера аутентификации.
    /// </summary>
    public class AuthOptions
    {
        public const string SectionName = "auth";

        public const int DefaultUserCacheTimeout = 30;

        public const string DefaultLoginPath = "/";

        public const string DefaultLogoutPath = "/";

        public static readonly AuthOptions Default = new AuthOptions();


        public AuthOptions()
        {
            UserCacheTimeout = DefaultUserCacheTimeout;
            LoginPath = DefaultLoginPath;
            LogoutPath = DefaultLogoutPath;
        }


        /// <summary>
        /// Таймаут сброса кэша пользователей в минутах.
        /// </summary>
        public int UserCacheTimeout { get; set; }

        /// <summary>
        /// Путь для перенаправления при возврате 401 Unauthorized.
        /// </summary>
        public string LoginPath { get; set; }

        /// <summary>
        /// Путь для перенаправления при выходе из системы.
        /// </summary>
        public string LogoutPath { get; set; }

        /// <summary>
        /// Фабрика для получения хранилища пользователей.
        /// </summary>
        public ICustomUserStoreFactory UserStoreFactory { get; set; }
    }
}