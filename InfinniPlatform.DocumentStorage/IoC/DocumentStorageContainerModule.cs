using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.DocumentStorage.Diagnostics;
using InfinniPlatform.DocumentStorage.Hosting;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.Services;
using InfinniPlatform.DocumentStorage.Services.QueryFactories;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Storage;
using InfinniPlatform.DocumentStorage.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Transactions;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Session;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.DocumentStorage.IoC
{
    internal sealed class DocumentStorageContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // MongoDB

            builder.RegisterFactory(GetMongoConnectionSettings)
                   .As<MongoConnectionSettings>()
                   .SingleInstance();

            builder.RegisterType<MongoConnection>()
                   .As<MongoConnection>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentIdGenerator>()
                   .As<IDocumentIdGenerator>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentStorageManager>()
                   .As<IDocumentStorageManager>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentStorageProvider>()
                   .As<IDocumentStorageProvider>()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(MongoDocumentStorageProvider<>))
                   .As(typeof(IDocumentStorageProvider<>))
                   .InstancePerDependency();

            // Storage

            builder.RegisterType<DocumentStorageProviderFactory>()
                   .As<IDocumentStorageProviderFactory>()
                   .SingleInstance();

            builder.RegisterType<DocumentStorageIdProvider>()
                   .As<IDocumentStorageIdProvider>()
                   .SingleInstance();

            builder.RegisterType<DocumentStorageHeaderProvider>()
                   .As<IDocumentStorageHeaderProvider>()
                   .SingleInstance();

            builder.RegisterType<DocumentStorageFilterProvider>()
                   .As<IDocumentStorageFilterProvider>()
                   .SingleInstance();

            builder.RegisterType<DocumentStorageInterceptorProvider>()
                   .As<IDocumentStorageInterceptorProvider>()
                   .SingleInstance();

            builder.RegisterType<DocumentStorageImpl>()
                   .As<IDocumentStorage>()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(DocumentStorageImpl<>))
                   .As(typeof(IDocumentStorage<>))
                   .InstancePerDependency();

            // System storage

            builder.RegisterType<SystemDocumentStorageFactory>()
                  .As<ISystemDocumentStorageFactory>()
                  .SingleInstance();

            builder.RegisterType<SystemDocumentStorageHeaderProvider>()
                   .As<ISystemDocumentStorageHeaderProvider>()
                   .SingleInstance();

            builder.RegisterType<SystemDocumentStorageFilterProvider>()
                   .As<ISystemDocumentStorageFilterProvider>()
                   .SingleInstance();

            builder.RegisterType<SystemDocumentStorageImpl>()
                   .As<ISystemDocumentStorage>()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(SystemDocumentStorageImpl<>))
                   .As(typeof(ISystemDocumentStorage<>))
                   .InstancePerDependency();

            // Transactions

            builder.RegisterType<DocumentStorageFactory>()
                   .As<IDocumentStorageFactory>()
                   .SingleInstance();

            builder.RegisterType<UnitOfWork>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<UnitOfWorkFactory>()
                   .As<IUnitOfWorkFactory>()
                   .SingleInstance();

            // SaaS

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();

            builder.RegisterType<SystemTenantProvider>()
                   .As<ISystemTenantProvider>()
                   .SingleInstance();

            // Services

            builder.RegisterType<QuerySyntaxTreeParser>()
                   .As<IQuerySyntaxTreeParser>()
                   .SingleInstance();

            builder.RegisterType<DocumentQueryFactory>()
                   .As<IDocumentQueryFactory>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(DocumentQueryFactory<>))
                   .As(typeof(IDocumentQueryFactory<>))
                   .SingleInstance();

            builder.RegisterType<DocumentHttpService>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(DocumentHttpService<>))
                   .As(typeof(DocumentHttpService<>))
                   .InstancePerDependency();

            builder.RegisterType<DocumentHttpServiceFactory>()
                   .As<IDocumentHttpServiceFactory>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceWrapperFactory>()
                   .As<IHttpServiceWrapperFactory>()
                   .SingleInstance();

            builder.RegisterType<DocumentHttpServiceSource>()
                   .As<IHttpServiceSource>()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<MongoCollectionInitializer>()
                   .As<IAppEventHandler>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<MongoStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }


        private static MongoConnectionSettings GetMongoConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<MongoConnectionSettings>(MongoConnectionSettings.SectionName);
        }
    }
}