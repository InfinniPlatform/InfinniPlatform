using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.ElasticSearch.Versioning;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.ElasticSearch.IoC
{
    internal sealed class ElasticSearchContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ElasticConnection>()
                   .As<IElasticConnection>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ElasticTypeManager>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<ElasticSearchSettings>(ElasticSearchSettings.SectionName))
                   .As<ElasticSearchSettings>()
                   .SingleInstance();

            // For ElasticFactory

            builder.RegisterType<ElasticFactory>()
                   .As<IIndexFactory>()
                   .SingleInstance();

            builder.RegisterType<IndexQueryExecutor>()
                   .As<IIndexQueryExecutor>()
                   .InstancePerDependency();

            builder.RegisterType<VersionBuilder>()
                   .As<IVersionBuilder>()
                   .InstancePerDependency();

            builder.RegisterType<ElasticSearchProvider>()
                   .As<ICrudOperationProvider>()
                   .InstancePerDependency();

            builder.RegisterType<ElasticSearchAggregationProvider>()
                   .As<IAggregationProvider>()
                   .InstancePerDependency();

            builder.RegisterType<IndexToTypeAccordanceProvider>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ElasticSearchProviderAllIndexes>()
                   .As<IAllIndexesOperationProvider>()
                   .SingleInstance();
        }
    }
}