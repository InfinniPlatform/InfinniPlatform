using InfinniPlatform.ContextComponents;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Index.ElasticSearch.IoC
{
    internal sealed class ElasticSearchContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ElasticFactory>()
                   .As<IIndexFactory>()
                   .SingleInstance();

            builder.RegisterType<IndexComponent>()
                   .As<IIndexComponent>()
                   .SingleInstance();

            builder.RegisterType<ElasticConnection>()
                   .As<IElasticConnection>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}