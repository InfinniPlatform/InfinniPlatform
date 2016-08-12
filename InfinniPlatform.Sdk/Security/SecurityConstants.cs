namespace InfinniPlatform.Sdk.Security
{
    /// <summary>
    /// Константы пространства имен <see cref="InfinniPlatform.Sdk.Security"/>.
    /// </summary>
    public static class SecurityConstants
    {
        /// <summary>
        /// Идентификатор организации для системных пользователей.
        /// </summary>
        public const string SystemUserTenantId = "system";

        /// <summary>
        /// Идентификатор организации для пользователей с неопределенной организацией.
        /// </summary>
        public const string UndefinedUserTenantId = "undefined";

        /// <summary>
        /// Идентификатор организации для анонимных пользователей (пользователей, не прошедших аутентификацию).
        /// </summary>
        public const string AnonymousUserTenantId = "anonymous";
    }
}