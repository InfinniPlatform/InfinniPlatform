namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Настройки модуля <see cref="ExternalAuthAdfsHttpMiddleware"/>.
    /// </summary>
    internal sealed class ExternalAuthAdfsOwinHostingModuleSettings
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