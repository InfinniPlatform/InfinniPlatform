﻿namespace InfinniPlatform.Security
{
    /// <summary>
    /// Типы утверждений системы.
    /// </summary>
    /// <remarks>
    /// Расширение системного списка <see cref="System.Security.Claims.ClaimTypes" />.
    /// </remarks>
    public static class AppClaimTypes
    {
        /// <summary>
        /// Тип утверждения для хранения идентификатора текущей организации.
        /// </summary>
        public const string TenantId = "tenantid";

        /// <summary>
        /// Тип утверждения для хранения идентификатора организации по умолчанию.
        /// </summary>
        public const string DefaultTenantId = "defaulttenantid";
    }
}