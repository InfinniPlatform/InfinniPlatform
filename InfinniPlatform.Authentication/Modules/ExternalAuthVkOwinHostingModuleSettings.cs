namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="ExternalAuthVkHttpMiddleware"/>.
    /// </summary>
    internal sealed class ExternalAuthVkOwinHostingModuleSettings
    {
        public const string SectionName = "authVk";


        /// <summary>
        /// Разрешает аутентификацию через ВКонтакте.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// ClientId приложения в ВКонтакте.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret приложения в ВКонтакте.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}