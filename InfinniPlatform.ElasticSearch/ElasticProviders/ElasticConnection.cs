using System;
using System.Linq;

using Elasticsearch.Net.ConnectionPool;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.IndexTypeVersions;

using Nest;
using Nest.Resolvers;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Соединение с ElasticSearch.
    /// </summary>
    public sealed class ElasticConnection : IElasticConnection
    {
        public ElasticConnection(ElasticSearchSettings settings)
        {
            _settings = settings;
            _elasticClient = new Lazy<ElasticClient>(() => CreatElasticClient(settings));
        }


        private readonly ElasticSearchSettings _settings;
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

            var client = new ElasticClient(connectionSettings);

            return client;
        }


        public ElasticClient Client => _elasticClient.Value;


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