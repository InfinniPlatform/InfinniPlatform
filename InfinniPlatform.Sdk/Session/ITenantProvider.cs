namespace InfinniPlatform.Sdk.Session
{
    /// <summary>
    /// Предоставляет методы определения идентификатора организации пользователя системы по модели SaaS.
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Возвращает идентификатор организации пользователя системы по модели SaaS.
        /// </summary>
        string GetTenantId();
    }
}