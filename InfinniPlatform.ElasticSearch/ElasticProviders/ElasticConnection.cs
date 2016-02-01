using System;
using System.Linq;

using Elasticsearch.Net.ConnectionPool;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;
using InfinniPlatform.Sdk.Logging;

using Nest;
using Nest.Resolvers;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Соединение с ElasticSearch.
    /// </summary>
    [LoggerName("ElasticSearch")]
    public sealed class ElasticConnection : IElasticConnection
    {
        public ElasticConnection(ElasticSearchSettings settings, IPerformanceLog performanceLog)
        {
            _settings = settings;
            _performanceLog = performanceLog;
            _elasticClient = new Lazy<ElasticClient>(() => CreatElasticClient(settings));
        }


        private readonly ElasticSearchSettings _settings;
        private readonly IPerformanceLog _performanceLog;
        private readonly Lazy<ElasticClient> _elasticClient;


        private static ElasticClient CreatElasticClient(ElasticSearchSettings settings)
        {
            var nodeAddresses = settings.Nodes.Select(node => new Uri(node));

            var connectionPool = new SniffingConnectionPool(nodeAddresses);

            var connectionSettings = new ConnectionSettings(connectionPool);

            if (!string.IsNullOrEmpty(settings.Login) && !string.IsNullOrEmpty(settings.Password))
            {
                connectionSettings.SetBasicAuthentication(settings.Login, settings.Password);
            }

            connectionSettings.SetDefaultPropertyNameInferrer(i => i);
            connectionSettings.SetJsonSerializerSettingsModifier(m => m.ContractResolver = new ElasticContractResolver(connectionSettings));
            connectionSettings.EnableTrace(settings.EnableTrace);

            var client = new ElasticClient(connectionSettings);

            return client;
        }


        public ElasticClient Client => _elasticClient.Value;

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector) where T : class
        {
            var start = DateTime.Now;
            try
            {
                var searchResponse = Client.Search(searchSelector);

                _performanceLog.Log("Search", start);

                return searchResponse;
            }
            catch (Exception e)
            {
                _performanceLog.Log("Search", start, e);
                throw;
            }
        }

        public IBulkResponse Bulk(Func<BulkDescriptor, BulkDescriptor> bulkSelector)
        {
            var start = DateTime.Now;

            try
            {
                var bulkResponse = Client.Bulk(bulkSelector);

                _performanceLog.Log("Bulk", start);

                return bulkResponse;
            }
            catch (Exception e)
            {
                _performanceLog.Log("Bulk", start, e);
                throw;
            }
        }

        [Obsolete]
        public void Refresh()
        {
            // TODO: От этого нужно избавиться!!!
            Client.Refresh(i => i.Force());
        }

        public string GetIndexName(string configuration)
        {
            var indexName = configuration.ToLower();

            return indexName;
        }

        public string GetBaseTypeName(string documentType)
        {
            var baseTypeName = documentType.ToLower() + IndexTypeMapper.MappingTypeVersionPattern;

            return baseTypeName;
        }

        public string GetStatus()
        {
            var health = _elasticClient.Value.ClusterHealth();
            var nodeAddresses = string.Join("; ", _settings.Nodes);

            return $"cluster name - {health.ClusterName}, status - {health.Status}, number of nodes: {health.NumberOfNodes}, configured nodes: {nodeAddresses}";
        }
    }
}