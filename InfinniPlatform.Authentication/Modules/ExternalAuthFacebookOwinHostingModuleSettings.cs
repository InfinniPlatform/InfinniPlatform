namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="ExternalAuthFacebookOwinHostingMiddleware"/>.
    /// </summary>
    internal sealed class ExternalAuthFacebookOwinHostingModuleSettings
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