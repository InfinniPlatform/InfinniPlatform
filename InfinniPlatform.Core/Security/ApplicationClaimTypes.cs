namespace InfinniPlatform.Core.Security
{
    /// <summary>
    ///     Типы утверждений системы.
    /// </summary>
    /// <remarks>
    ///     Расширение системного списка <see cref="System.Security.Claims.ClaimTypes" />.
    /// </remarks>
    public static class ApplicationClaimTypes
    {
        /// <summary>
        ///     Активная роль пользователя.
        /// </summary>
        public const string ActiveRole = "http://infinniplatform/identity/claims/activerole";

        /// <summary>
        ///     Роль пользователя по умолчанию.
        /// </summary>
        public const string DefaultRole = "http://infinniplatform/identity/claims/defaultrole";
    }
}