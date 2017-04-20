using InfinniPlatform.Diagnostics;
using InfinniPlatform.DocumentStorage.Diagnostics;
using InfinniPlatform.DocumentStorage.Hosting;
using InfinniPlatform.DocumentStorage.Transactions;
using InfinniPlatform.Hosting;
using InfinniPlatform.IoC;

namespace InfinniPlatform.DocumentStorage.IoC
{
    public class MongoDocumentStorageContainerModule : IContainerModule
    {
        public MongoDocumentStorageContainerModule(MongoDocumentStorageOptions options)
        {
            _options = options;
        }

        private readonly MongoDocumentStorageOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            // MongoDB

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

            // Session

            builder.RegisterType<SystemTenantProvider>()
                   .As<ISystemTenantProvider>()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<MongoDocumentStorageInitializer>()
                   .As<IAppInitHandler>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<MongoDocumentStorageStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }
    }
}