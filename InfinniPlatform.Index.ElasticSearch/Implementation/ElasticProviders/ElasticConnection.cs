using System;
using System.Collections.Generic;
using System.Linq;

using Elasticsearch.Net.ConnectionPool;

using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Sdk.Environment.Settings;

using Nest;
using Nest.Resolvers;

using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	/// <summary>
	/// Соединение с ElasticSearch.
	/// </summary>
	public sealed class ElasticConnection
	{
		private static readonly NodePool NodePool;


		static ElasticConnection()
		{
			var nodeAddresses = AppSettings.GetValues("ElasticSearchNodes", "http://localhost:9200").Select(url => new Uri(url.Trim()));

			NodePool = new NodePool(nodeAddresses);
		}

		public ElasticConnection()
		{
			var nodeAddresses = NodePool.GetActualNodes();
			var connectionPool = new SniffingConnectionPool(nodeAddresses);
			var connectionSettings = new ConnectionSettings(connectionPool);
			connectionSettings.SetDefaultPropertyNameInferrer(i => i);
			connectionSettings.SetJsonSerializerSettingsModifier(m => m.ContractResolver = new ElasticContractResolver(connectionSettings));

			_client = new ElasticClient(connectionSettings);
		}


		private readonly ElasticClient _client;


		public void ConnectIndex()
		{
			//if (!_client.Connection.HeadSync(_firstNodeUrl).Success)
			//{
			//	throw new Exception(Resources.CantConnectElasticSearch);
			//}
		}


		/// <summary>
		/// Возвращает клиента для работы с индексом.
		/// </summary>
		public ElasticClient Client
		{
			get { return _client; }
		}

		/// <summary>
		/// Возвращает все типы, являющиеся версиями указанного типа в индексе
		/// </summary>
		/// <param name="indexNames">Наименования индексов, по которым выполняем поиск.</param>
		/// <param name="typeNames">Типы данных, для которого получаем все версии типов в индексе.</param>
		/// <returns>Список всех версий типов данных в указанных индексах.</returns>
		/// <remarks>
		/// Например product_schema_0, product_schema_1 являются версиями типа product и искать данные можно по всем этим типам.
		/// </remarks>
		public IEnumerable<IndexToTypeAccordance> GetAllTypes(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
		{
			if (typeNames == null || !typeNames.Any())
			{
				throw new ArgumentException("Type name for index should not be empty.");
			}


			// Ищем все маппинги для указанных типов (используем низкоуровневый вызов GetMapping).
			// Ищем по всем типам и всем указанным индексам.
			return GetTypeVersions(indexNames, typeNames);
		}

		/// <summary>
		/// Возвращает актуальную схему для документов, хранимых в типе.
		/// </summary>
		public IList<PropertyMapping> GetIndexTypeMapping(string indexName, string typeName)
		{
			var schemaTypes = GetAllTypes(new[] { indexName }, new[] { typeName });

			if (schemaTypes == null || !schemaTypes.Any())
			{
				return new PropertyMapping[] { };
			}

			var actualType = schemaTypes.GetActualTypeName(typeName);

			return ElasticMappingExtension.GetIndexTypeMapping(NodePool, indexName, actualType);
		}

		/// <summary>
		/// Получение статуса кластера.
		/// </summary>
		public string GetStatus()
		{
			var health = _client.ClusterHealth();
			var nodeAddresses = string.Join("; ", NodePool.GetActualNodes().Select(i => i.ToString()));

			return string.Format("cluster name - {0}, status - {1}, number of nodes: {2}, configured nodes: {3}", health.ClusterName, health.Status, health.NumberOfNodes, nodeAddresses);
		}

		/// <summary>
		/// Возвращает список версий указанных типов в указанных индексах.
		/// </summary>
		/// <param name="indexNames">Список индексов для поиска</param>
		/// <param name="typeNames">Список типов, для которых ищем версии схем</param>
		/// <returns>Список версий схем</returns>
		private static IEnumerable<IndexToTypeAccordance> GetTypeVersions(IEnumerable<string> indexNames, IEnumerable<string> typeNames)
		{
			// собираем маппинги по всем индексам и типам
			var mappings = ElasticMappingExtension.FillIndexMappings(NodePool, indexNames, typeNames);
			typeNames = typeNames.Select(t => t.ToLowerInvariant()).ToList();
			indexNames = indexNames.Select(s => s.ToLowerInvariant()).ToList();

			return mappings.Where(m => indexNames.Contains(m.IndexName) && typeNames.Contains(m.BaseTypeName))
						   .OrderBy(m => m.IndexName)
						   .ThenBy(m => m.BaseTypeName)
						   .ToList();
		}
	}
}