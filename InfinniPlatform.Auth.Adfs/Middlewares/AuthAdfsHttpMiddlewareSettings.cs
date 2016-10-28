namespace InfinniPlatform.Auth.Adfs.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthAdfsHttpMiddleware"/>.
    /// </summary>
    public class AuthAdfsHttpMiddlewareSettings
    {
        public const string SectionName = "authAdfs";


        /// <summary>
        /// Разрешает аутентификацию через ADFS.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Адрес сервера ADFS.
        /// </summary>
        /// <example>
        /// myadfs.org:1234
        /// </example>
        public string Server { get; set; }

        /// <summary>
        /// URI защищенного ресурса.
        /// </summary>
        /// <example>
        /// https://InfinniPlatform
        /// </example>
        public string ResourceUri { get; set; }
    }
}