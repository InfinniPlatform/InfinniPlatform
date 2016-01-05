namespace InfinniPlatform.Core.Security
{
    /// <summary>
    ///     Утверждение пользователя системы.
    /// </summary>
    public sealed class ApplicationUserClaim
    {
        /// <summary>
        ///     Тип утверждения.
        /// </summary>
        /// <remarks>
        ///     Ссылка на тип утверждения.
        /// </remarks>
        /// <example>
        ///     { Id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone', DisplayName: 'Мобильный телефон' }
        /// </example>
        public ForeignKey Type { get; set; }

        /// <summary>
        ///     Значение утверждения.
        /// </summary>
        /// <remarks>
        ///     Значение утверждения заданного типа для пользователя системы.
        /// </remarks>
        /// <example>
        ///     +7 (123) 456-78-90
        /// </example>
        public string Value { get; set; }
    }
}