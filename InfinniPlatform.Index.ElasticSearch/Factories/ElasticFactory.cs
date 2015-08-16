using InfinniPlatform.Api.Index;
using InfinniPlatform.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning;
using System.Collections.Generic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Factories
{
	/// <summary>
	///   Фабрика для получения инициализированных, готовых к работе
	///   реализаций контрактов работы с данными ElasticSearch
	/// </summary>
    public sealed class ElasticFactory : IIndexFactory
    {
		private readonly IMultitenancyProvider _multitenancyProvider;
	    private readonly IndexToTypeAccordanceProvider _accordanceProvider;

		public ElasticFactory(IMultitenancyProvider multitenancyProvider)
		{
		    _multitenancyProvider = multitenancyProvider;
		    _accordanceProvider = new IndexToTypeAccordanceProvider();
		}


	    /// <summary>
        ///   Создать конструктор версий хранилища документов
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа</param>
        /// <param name="searchAbilityType">Тип поиска по индексу</param>
        public IVersionBuilder BuildVersionBuilder(string indexName, string typeName, SearchAbilityType searchAbilityType)
        {
            return new VersionBuilder(new IndexStateProvider(), indexName, typeName,searchAbilityType);
        }

	    /// <summary>
	    ///   Создать провайдер данных
	    /// </summary>
	    /// <param name="indexName">Наименование индекса</param>
	    /// <param name="typeName">Наименование типа</param>
        /// <param name="tenantId">Идентификатор организации-клиента для получения данных</param>
        /// <param name="version">Версия данных</param>

	    public IVersionProvider BuildVersionProvider(string indexName, string typeName, string tenantId, string version = null)
		{

			var expectedtenantId = _multitenancyProvider.GetTenantId(tenantId, indexName, typeName);
	        return new VersionProvider(
	            new ElasticSearchProvider(indexName, typeName, expectedtenantId, version), 
                new DocumentProvider(new IndexQueryExecutor(_accordanceProvider.GetIndexTypeAccordances(new [] { indexName }, new []{ typeName}), expectedtenantId)));
	    }

	    /// <summary>
	    ///   Создать провайдер данных для доступа к нескольким индексам
	    /// </summary>
        /// <param name="tenantId">Идентификатор организации-клиента для выполнения запросов</param>
	    /// <param name="indexNames">Наименование индексов. Если имена не указаны,
	    ///     для поиска будут использованы все имеющиеся индексы</param>
	    /// <param name="typeNames">Наименования типов</param>
	    public IDocumentProvider BuildMultiIndexDocumentProvider(string tenantId, IEnumerable<string> indexNames = null, IEnumerable<string> typeNames = null)
        {
            // Создаём универсальный провайдер для выполнения поисковых запросов ко всем документам конфигурации
            return new DocumentProvider(new IndexQueryExecutor(_accordanceProvider.GetIndexTypeAccordances(indexNames, typeNames), tenantId));
        }
        
		private readonly List<ElasticSearchProviderInfo> _providersInfo = new List<ElasticSearchProviderInfo>();

	    /// <summary>
	    ///   Создать провайдер для поиска данных
	    /// </summary>
	    /// <param name="indexName">Наименование индекса для поиска</param>
	    /// <param name="typeName">Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех существующих в индексе типов</param>
        /// <param name="tenantId">Идентификатор организации-клиента для выполнения запросов</param>
	    /// <param name="version">Версия конфигурации, для которой создается провайдер данных</param>
	    /// <returns>Провайдер для поиска данных</returns>
		public ICrudOperationProvider BuildCrudOperationProvider(string indexName, string typeName, string tenantId, string version = null)
		{
			var expectedtenantId = _multitenancyProvider.GetTenantId(tenantId, indexName, typeName);
	 	    var providerInfo = _providersInfo.FindInfo(indexName, typeName);
	 	    if (providerInfo == null)
	 	    {
		 	    var provider =  new ElasticSearchProvider(indexName, typeName, expectedtenantId, version);
				_providersInfo.Add(new ElasticSearchProviderInfo(indexName,typeName,provider));
		 	    return provider;
	 	    }
	 	    return providerInfo.Provider;
 	    }

        /// <summary>
        ///   Получить провайдер операций по всем индексам и типам базы
        /// </summary>
        /// <param name="tenantId">Идентификатор организации-клиента для выполнения запросов</param>
        /// <returns>Провайдер операций по всем индексам и типам базы</returns>
	    public IAllIndexesOperationProvider BuildAllIndexesOperationProvider(string tenantId)
	    {
            var expectedTenantId = _multitenancyProvider.GetTenantId(tenantId);
            return new ElasticSearchProviderAllIndexes(expectedTenantId);
	    }

	    /// <summary>
	    ///   Создать провайдер операций для работы с индексами
	    /// </summary>
	    /// <returns>Провайдер операций для работы с индексом</returns>
	    public IIndexStateProvider BuildIndexStateProvider()
        {
            return new IndexStateProvider();            
        }

		/// <summary>
		///   Создать исполнитель запросов к индексу
		/// </summary>
		/// <param name="indexName">Наимемнование индекса, для которого выполняется запрос. Если не указан, осуществляется выборка из всех существующих индексов</param>
		/// <param name="typeName">Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка из всех существующих в индексе типов</param>
        /// <param name="tenantId">Идентификатор организации-клиента выполнения запросов</param>
		/// <returns></returns>
		public IIndexQueryExecutor BuildIndexQueryExecutor(string indexName, string typeName, string tenantId)
	    {
			var expectedtenantId = _multitenancyProvider.GetTenantId(tenantId, indexName, typeName);
            return new IndexQueryExecutor(_accordanceProvider.GetIndexTypeAccordances(new [] {indexName}, new [] {typeName}), expectedtenantId);
	    }

		/// <summary>
		///   Создать исполнитель агрегационных запросов к индексу
		/// </summary>
		/// <param name="indexName">Наимемнование индекса, для которого выполняется запрос</param>
		/// <param name="typeName">Наименование типа для выполнения операций с данными. Если не указан, осуществляется выборка всех существующих в индексе типов</param>
        /// <param name="tenantId">Идентификатор организации-клиента выполнения запросов</param>
		public IAggregationProvider BuildAggregationProvider(string indexName, string typeName, string tenantId)
        {
            var expectedTenantId = _multitenancyProvider.GetTenantId(tenantId, indexName, typeName);
            return new ElasticSearchAggregationProvider(indexName, typeName, expectedTenantId);
        }
    }
}
