namespace InfinniPlatform.Auth.Cookie.Middlewares
{
    /// <summary>
    /// Настройки для <see cref="AuthCookieHttpMiddleware"/>.
    /// </summary>
    public class AuthCookieHttpMiddlewareSettings
    {
        public const string SectionName = "authCookie";


        public AuthCookieHttpMiddlewareSettings()
        {
            LoginPath = "/Auth/SignInInternal";
            LogoutPath = "/Auth/SignOut";
        }


        /// <summary>
        /// Домен для создания Cookie.
        /// </summary>
        public string CookieDomain { get; set; }

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