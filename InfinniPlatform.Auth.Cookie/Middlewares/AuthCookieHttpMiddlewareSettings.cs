namespace InfinniPlatform.Auth.Cookie.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthCookieHttpMiddleware"/>.
    /// </summary>
    public class AuthCookieHttpMiddlewareSettings
    {
        public const string SectionName = "authCookie";


        public AuthCookieHttpMiddlewareSettings() {}


        /// <summary>
        /// Домен для создания Cookie.
        /// </summary>
        public string CookieDomain { get; set; }
    }
}