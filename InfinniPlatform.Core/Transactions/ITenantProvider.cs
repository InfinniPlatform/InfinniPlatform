namespace InfinniPlatform.Core.Transactions
{
    /// <summary>
    /// Предоставляет методы определения пользователя системы по модели SaaS.
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Возвращает идентификатор пользователя системы по модели SaaS.
        /// </summary>
        string GetTenantId();
    }
}