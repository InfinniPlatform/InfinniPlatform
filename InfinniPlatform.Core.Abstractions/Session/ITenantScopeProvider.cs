namespace InfinniPlatform.Core.Session
{
    /// <summary>
    /// Предоставляет методы для создания экземпляров <see cref="ITenantScope" />.
    /// </summary>
    public interface ITenantScopeProvider
    {
        /// <summary>
        /// Возвращает текущую область выполнения запросов.
        /// </summary>
        ITenantScope GetTenantScope();

        /// <summary>
        /// Начинает новую область выполнения запросов.
        /// </summary>
        /// <param name="tenantId">Идентификатор организации.</param>
        ITenantScope BeginTenantScope(string tenantId);
    }
}