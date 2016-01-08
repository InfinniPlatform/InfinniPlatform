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
            TenantProvider = new Lazy<ITenantProvider>(CreateTenantProvider);
        }


        private static readonly Lazy<ElasticConnection> ElasticConnection;
        private static readonly Lazy<ITenantProvider> TenantProvider;


        public static ElasticFactory GetElasticFactory()
        {
            return new ElasticFactory(
                settings => new IndexQueryExecutor(ElasticConnection.Value, TenantProvider.Value, settings),
                (indexName, typeName) => new VersionBuilder(ElasticConnection.Value, indexName, typeName),
                (indexName, typeName) => new ElasticSearchProvider(ElasticConnection.Value, TenantProvider.Value, indexName, typeName),
                (indexName, typeName) => new ElasticSearchAggregationProvider(ElasticConnection.Value, TenantProvider.Value, indexName, typeName),
                new IndexToTypeAccordanceProvider(ElasticConnection.Value),
                new ElasticSearchProviderAllIndexes(ElasticConnection.Value));
        }


        private static ElasticConnection CreateElasticConnection()
        {
            return new ElasticConnection();
        }

        private static ITenantProvider CreateTenantProvider()
        {
            var tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(i => i.GetTenantId(It.IsAny<string>())).Returns("anonimous");
            return tenantProviderMock.Object;
        }
    }
}