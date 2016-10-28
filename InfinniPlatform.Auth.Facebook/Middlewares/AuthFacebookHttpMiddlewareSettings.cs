namespace InfinniPlatform.Auth.Facebook.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthFacebookHttpMiddleware"/>.
    /// </summary>
    public class AuthFacebookHttpMiddlewareSettings
    {
        public const string SectionName = "authFacebook";


        /// <summary>
        /// Разрешает аутентификацию через Facebook.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// ClientId приложения в Facebook.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret приложения в Facebook.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}