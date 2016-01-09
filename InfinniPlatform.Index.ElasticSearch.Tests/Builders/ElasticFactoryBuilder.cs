using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning;

using Moq;

namespace InfinniPlatform.Index.ElasticSearch.Tests.Builders
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
                settings => new IndexQueryExecutor(ElasticConnection.Value, TenantProvider.Value, settings),
                (indexName, typeName) => new VersionBuilder(ElasticTypeManager.Value, indexName, typeName),
                (indexName, typeName) => new ElasticSearchProvider(ElasticConnection.Value, ElasticTypeManager.Value, TenantProvider.Value, indexName, typeName),
                (indexName, typeName) => new ElasticSearchAggregationProvider(ElasticConnection.Value, ElasticTypeManager.Value, TenantProvider.Value, indexName, typeName),
                new IndexToTypeAccordanceProvider(ElasticTypeManager.Value),
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
            tenantProviderMock.Setup(i => i.GetTenantId(It.IsAny<string>())).Returns("anonimous");
            return tenantProviderMock.Object;
        }
    }
}