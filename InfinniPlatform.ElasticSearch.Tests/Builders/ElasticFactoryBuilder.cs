using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Factories;

using Moq;

namespace InfinniPlatform.ElasticSearch.Tests.Builders
{
    // TODO: Избавиться от этого после уничтожения ElasticFactory

    public static class ElasticFactoryBuilder
    {
        static ElasticFactoryBuilder()
        {
            ElasticConnection = new Lazy<ElasticConnection>(CreateElasticConnection);
            ElasticTypeManager = new Lazy<ElasticTypeManager>(CreateElasticTypeManager);
            TenantProvider = new Lazy<ITenantProvider>(CreateTenantProvider);
        }


        public static readonly Lazy<ElasticConnection> ElasticConnection;
        public static readonly Lazy<ElasticTypeManager> ElasticTypeManager;
        public static readonly Lazy<ITenantProvider> TenantProvider;


        public static ElasticFactory GetElasticFactory()
        {
            return new ElasticFactory(
                (indexName, typeName) => new IndexQueryExecutor(ElasticConnection.Value, TenantProvider.Value, ElasticTypeManager.Value, indexName, typeName),
                (indexName, typeName) => new ElasticSearchAggregationProvider(ElasticConnection.Value, ElasticTypeManager.Value, TenantProvider.Value, indexName, typeName),
                new ElasticSearchProviderAllIndexes(ElasticConnection.Value));
        }


        private static ElasticConnection CreateElasticConnection()
        {
            return new ElasticConnection(ElasticSearchSettings.Default);
        }

        private static ElasticTypeManager CreateElasticTypeManager()
        {
            return new ElasticTypeManager(ElasticConnection.Value);
        }

        private static ITenantProvider CreateTenantProvider()
        {
            var tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(i => i.GetTenantId()).Returns("anonimous");
            return tenantProviderMock.Object;
        }
    }
}