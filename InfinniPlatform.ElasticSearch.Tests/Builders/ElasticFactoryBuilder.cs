using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

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
                typeName => new IndexQueryExecutor(ElasticConnection.Value, ElasticTypeManager.Value, TenantProvider.Value, typeName),
                typeName => new ElasticSearchAggregationProvider(ElasticConnection.Value, ElasticTypeManager.Value, TenantProvider.Value, typeName),
                new ElasticSearchProviderAllIndexes(ElasticConnection.Value));
        }


        private static ElasticConnection CreateElasticConnection()
        {
            var appEnvironment = new Mock<IAppEnvironment>();
            var performanceLog = new Mock<IPerformanceLog>();
            appEnvironment.SetupGet(i => i.Name).Returns("InfinniPlatform");

            return new ElasticConnection(appEnvironment.Object, ElasticSearchSettings.Default, performanceLog.Object);
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