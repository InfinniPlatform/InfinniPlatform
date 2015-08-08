
namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Провайдер многопользовательского режима
	/// </summary>
    public interface IMultitenancyProvider
	{
		/// <summary>
        ///   Получить идентификатор организации-клиента в зависимости от индекса, к которому происходит обращение
		/// </summary>
		/// <param name="tenantId">Идентификатор, предоставленный пользователем</param>
		/// <param name="indexName">Индекс для формирования роутинга</param>
		/// <param name="indexType">Тип в индексе для формирования роутинга</param>
		/// <returns>Строка роутинга для запросов к индексу</returns>
        string GetTenantId(string tenantId, string indexName = null, string indexType = null);
	}
}
