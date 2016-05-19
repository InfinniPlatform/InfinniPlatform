namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// Настройки хранилища пользователей.
    /// </summary>
    internal sealed class UserStorageSettings
    {
        public const string SectionName = "userStorage";

        public const int DefaultUserCacheTimeout = 30;


        public UserStorageSettings()
        {
            UserCacheTimeout = DefaultUserCacheTimeout;
        }


        /// <summary>
        /// Таймаут сброса кэша пользователей в минутах.
        /// </summary>
        public int UserCacheTimeout { get; set; }
    }
}