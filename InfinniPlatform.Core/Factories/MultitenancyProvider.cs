using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.Factories
{
    /// <summary>
    ///     Провайдер многопользовательского режима
    /// </summary>
    public static class MultitenancyProvider
    {
        /// <summary>
        ///     Получить идентификатор организации-клиента в зависимости от индекса, к которому происходит обращение
        /// </summary>
        /// <param name="tenantId">Идентификатор, предоставленный пользователем</param>
        /// <param name="indexName">Индекс для формирования идентификатора рганизации-клиента</param>
        /// <param name="indexType">Тип в индексе для формирования идентификатора организации-клиента</param>
        /// <returns>Строка роутинга для запросов к индексу</returns>
        public static string GetTenantId(string tenantId, string indexName = null, string indexType = null)
        {
            if (indexName != null && (indexName.ToLowerInvariant() == "systemconfig" ||
                                      indexName.ToLowerInvariant() == "authorization" ||
                                      indexName.ToLowerInvariant() == "restfulapi" ||
                                      indexName.ToLowerInvariant() == "update"))
            {
                return AuthorizationStorageExtensions.SystemTenant;
            }
            if (string.IsNullOrEmpty(tenantId))
            {
                return AuthorizationStorageExtensions.AnonimousUser;
            }

            return tenantId;
        }
    }
}