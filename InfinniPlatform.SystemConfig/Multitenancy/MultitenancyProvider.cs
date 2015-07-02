using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Factories;

namespace InfinniPlatform.SystemConfig.Multitenancy
{
    public sealed class MultitenancyProvider : IMultitenancyProvider
	{
        /// <summary>
        ///   Получить идентификатор организации-клиента в зависимости от индекса, к которому происходит обращение
        /// </summary>
        /// <param name="tenantId">Идентификатор, предоставленный пользователем</param>
        /// <param name="indexName">Индекс для формирования идентификатора рганизации-клиента</param>
        /// <param name="indexType">Тип в индексе для формирования идентификатора организации-клиента</param>
        /// <returns>Строка роутинга для запросов к индексу</returns>
        public string GetTenantId(string tenantId, string indexName, string indexType)
		{
			if (indexName.ToLowerInvariant() == "systemconfig" || 
                indexName.ToLowerInvariant() == "authorization" || 
                indexName.ToLowerInvariant() == "restfulapi" || 
                indexName.ToLowerInvariant() == "update")
			{
				return MultitenancyExtensions.SystemTenant;
			}
            if (string.IsNullOrEmpty(tenantId))
			{
				return AuthorizationStorageExtensions.AnonimousUser;
			}

            return tenantId;
		}
	}
}
