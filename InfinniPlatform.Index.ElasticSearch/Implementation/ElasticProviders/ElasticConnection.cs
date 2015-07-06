using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Properties;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers;
using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	/// <summary>
	///   Соединение с ElasticSearch
	/// </summary>
	public sealed class ElasticConnection
	{
		private readonly string _hostIp;
		private readonly int _port;

		private readonly ConnectionSettings _elasticSettings;

		private readonly ElasticClient _client;

		private const string UriPatternForConnection = "http://{0}:{1}";

		/// <summary>
		///   constructor base
		/// </summary>
		public ElasticConnection()
		{
			_hostIp = AppSettings.GetValue("ElasticSearchServerName", "localhost");
			_port = AppSettings.GetValue("ElasticSearchServerPort", 9200);

			_elasticSettings = new ConnectionSettings(
				new Uri(string.Format(UriPatternForConnection, _hostIp, _port)));
            
		    _elasticSettings.SetDefaultPropertyNameInferrer(i => i);
            _elasticSettings.SetJsonSerializerSettingsModifier(
                m => m.ContractResolver = new ElasticContractResolver(_elasticSettings));

			_client = new ElasticClient(_elasticSettings);
			
		}


		public void ConnectIndex()
		{
            if (!_client.Connection.HeadSync(new Uri(string.Format(UriPatternForConnection, _hostIp, _port))).Success)
			{
				throw new Exception(Resources.CantConnectElasticSearch);
			}
		}


		/// <summary>
		/// Возвращает клиента, специально подготовленного для добавления 
		/// новых документов в индекс
		/// </summary>
		public ElasticClient Client
		{
			get { return _client; }
		}

		/// <summary>
		///   Получить все типы, являющиеся версиями указанного типа в индексе
		///  (например product_schema_0, product_schema_1 являются версиями типа product и искать данные можно по всем этим типам)
		/// </summary>
		/// <param name="indexBaseNames">Наименования индексов, по которым выполняем поиск</param>
		/// <param name="typeNames">Типы данных, для которого получаем все версии типов в индексе</param>
		/// <returns>Список всех версий типов данных в указанных индексах</returns>
		public IEnumerable<IndexToTypeAccordance> GetAllTypes(IEnumerable<string> indexBaseNames, IEnumerable<string> typeNames)
		{
			if (typeNames == null || !typeNames.Any())
			{
				throw new ArgumentException("Type name for index should not be empty.");
			}


			//ищем все маппинги для указанных типов (используем низкоуровневый вызов GetMapping)
			//ищем по всем типам и всем указанным индексам
            return GetTypeVersions(typeNames, indexBaseNames);

		}

        /// <summary>
        /// Получение актуальной схемы для документов, хранимых в типе
        /// </summary>
        public IList<PropertyMapping> GetIndexTypeMapping(string indexName, string typeName)
        {
            var schemaTypes = GetAllTypes(new[] {indexName}, new[] {typeName});

            if (schemaTypes == null || !schemaTypes.Any())
            {
                return new PropertyMapping[0];
            }

            var actualType = schemaTypes.GetActualTypeName(typeName);

            return ElasticMappingExtension.GetIndexTypeMapping(_hostIp, _port, indexName, actualType);
        }

		/// <summary>
		///   Возвращает список версий указанных типов в указанных индексах
		/// </summary>
		/// <param name="typeNames">Список типов, для которых ищем версии схем</param>
		/// <param name="searchingIndeces">Список индексов для поиска</param>
		/// <returns>Список версий схем</returns>
		private IEnumerable<IndexToTypeAccordance> GetTypeVersions(IEnumerable<string> typeNames, IEnumerable<string> searchingIndeces)
		{
			//собираем маппинги по всем индексам и типам
            var mappings = ElasticMappingExtension.FillIndexMappings(_hostIp, _port, typeNames, searchingIndeces);
			typeNames = typeNames.Select(t => t.ToLowerInvariant()).ToList();
			searchingIndeces = searchingIndeces.Select(s => s.ToLowerInvariant()).ToList();
			return
				mappings.Where(m => searchingIndeces.Contains(m.IndexName) && typeNames.Contains(m.BaseTypeName)).OrderBy(m => m.IndexName).ThenBy(m => m.BaseTypeName).ToList();
		}
	}
}