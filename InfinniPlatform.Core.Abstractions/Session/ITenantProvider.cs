using System.Security.Principal;

namespace InfinniPlatform.Core.Abstractions.Session
{
    /// <summary>
    /// Предоставляет методы определения идентификатора организации пользователя системы.
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Возвращает идентификатор организации для текущего пользователя системы.
        /// </summary>
        string GetTenantId();

        /// <summary>
        /// Возвращает идентификатор организации для указанного пользователя системы.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        string GetTenantId(IIdentity identity);
    }
}