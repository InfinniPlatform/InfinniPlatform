namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Настройки внутреннего провайдера аутентификации.
    /// </summary>
    public class AuthInternalOptions
    {
        public const string SectionName = "authInternal";

        public const int DefaultUserCacheTimeout = 30;

        public const string DefaultLoginPath = "/";

        public const string DefaultLogoutPath = "/";

        public static readonly AuthInternalOptions Default = new AuthInternalOptions();


        public AuthInternalOptions()
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
    }
}