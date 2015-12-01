namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="ExternalAuthEsiaOwinHostingModule"/>.
    /// </summary>
    internal sealed class ExternalAuthEsiaOwinHostingModuleSettings
    {
        public const string SectionName = "authEsia";


        /// <summary>
        /// Разрешает аутентификацию через ЕСИА.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Базовый адрес сервера ЕСИА.
        /// </summary>
        /// <example>
        /// https://esia.ru
        /// </example>
        public string Server { get; set; }

        /// <summary>
        /// ClientId приложения в ЕСИА.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret приложения в ЕСИА.
        /// </summary>
        /// <remarks>
        /// Используется Thumbprint сертификата приложения.
        /// </remarks>
        public string ClientSecret { get; set; }
    }
}