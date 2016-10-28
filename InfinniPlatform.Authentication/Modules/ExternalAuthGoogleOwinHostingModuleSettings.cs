namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="ExternalAuthGoogleHttpMiddleware"/>.
    /// </summary>
    internal sealed class ExternalAuthGoogleOwinHostingModuleSettings
    {
        public const string SectionName = "authGoogle";


        /// <summary>
        /// Разрешает аутентификацию через Google+.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// ClientId приложения в Google+.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret приложения в Google+.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}