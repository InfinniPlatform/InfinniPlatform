namespace InfinniPlatform.SystemConfig.UserStorage
{
    /// <summary>
    /// Настройки хранилища пользователей.
    /// </summary>
    internal sealed class UserStorageSettings
    {
        public const string SectionName = "userStorage";


        public UserStorageSettings()
        {
            UserCacheTimeout = 30;
        }


        /// <summary>
        /// Таймаут сброса кэша пользователей в минутах.
        /// </summary>
        public int UserCacheTimeout { get; set; }
    }
}