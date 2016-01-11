namespace InfinniPlatform.Core.Security
{
    /// <summary>
    /// Имя входа пользователя системы у внешнего провайдера.
    /// </summary>
    public sealed class ApplicationUserLogin
    {
        /// <summary>
        /// Провайдер входа в систему.
        /// </summary>
        /// <example>
        /// Google
        /// </example>
        public string Provider { get; set; }

        /// <summary>
        /// Ключ, представляющий имя входа для провайдера.
        /// </summary>
        /// <example>
        /// https://www.google.com/accounts/o8/id?id=AIxOawlYjDIea-6N19tDINhYaNLCxWiJ7o8Wc_o
        /// </example>
        public string ProviderKey { get; set; }
    }
}