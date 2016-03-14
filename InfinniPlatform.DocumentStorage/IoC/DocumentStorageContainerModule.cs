using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Documents;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.DocumentStorage.Diagnostics;
using InfinniPlatform.DocumentStorage.Hosting;
using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.Obsolete;
using InfinniPlatform.DocumentStorage.Services;
using InfinniPlatform.DocumentStorage.Services.QueryFactory;
using InfinniPlatform.DocumentStorage.Services.QuerySyntax;
using InfinniPlatform.DocumentStorage.Storage;
using InfinniPlatform.DocumentStorage.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Transactions;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
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

            builder.RegisterFactory(GetMongoConnection)
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

            // Services

            builder.RegisterType<QuerySyntaxTreeParser>()
                   .As<IQuerySyntaxTreeParser>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(DocumentQueryFactory<>))
                   .As(typeof(IDocumentQueryFactory<>))
                   .SingleInstance();

            // Hosting

            builder.RegisterType<MongoCollectionInitializer>()
                   .As<IApplicationEventHandler>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<MongoStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();

            // Obsolete

            builder.RegisterType<MongoSetDocumentExecutor>()
                   .As<ISetDocumentExecutor>()
                   .SingleInstance();

            builder.RegisterType<MongoDocumentTransactionScope>()
                   .As<IDocumentTransactionScope>()
                   .InstancePerRequest();

            builder.RegisterType<MongoDocumentApi>()
                   .As<IDocumentApi>()
                   .SingleInstance();
        }


        private static MongoConnectionSettings GetMongoConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<MongoConnectionSettings>(MongoConnectionSettings.SectionName);
        }

        private static MongoConnection GetMongoConnection(IContainerResolver resolver)
        {
            return new MongoConnection(resolver.Resolve<IAppEnvironment>().Name, resolver.Resolve<MongoConnectionSettings>());
        }
    }
}