using InfinniPlatform.Core.Documents;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Core.SystemInfo;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.SystemConfig.Documents;
using InfinniPlatform.SystemConfig.Metadata;
using InfinniPlatform.SystemConfig.PrintView;
using InfinniPlatform.SystemConfig.Registers;
using InfinniPlatform.SystemConfig.Runtime;
using InfinniPlatform.SystemConfig.Services;
using InfinniPlatform.SystemConfig.StartupInitializers;
using InfinniPlatform.SystemConfig.SystemInfo;
using InfinniPlatform.SystemConfig.Transactions;
using InfinniPlatform.SystemConfig.UserStorage;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class SystemConfigContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Info

            builder.RegisterType<SystemInfoProvider>()
                   .As<ISystemInfoProvider>()
                   .SingleInstance();

            // Metadata

            builder.RegisterType<MetadataApi>()
                   .As<IMetadataApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ElasticTypesMigrationHelper>()
                   .AsSelf()
                   .SingleInstance();

            // Documents

            builder.RegisterType<DocumentApi>()
                   .As<IDocumentApi>()
                   .SingleInstance();

            builder.RegisterType<DocumentLinkMap>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<DocumentLinkMapProvider>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ReferenceResolver>()
                   .As<IReferenceResolver>()
                   .SingleInstance();

            builder.RegisterType<SetDocumentExecutor>()
                   .As<ISetDocumentExecutor>()
                   .SingleInstance();

            builder.RegisterType<GetDocumentExecutor>()
                   .As<IGetDocumentExecutor>()
                   .InstancePerDependency();

            // Transactions

            builder.RegisterType<DocumentTransactionScope>()
                   .As<IDocumentTransactionScope>()
                   .InstancePerRequest();

            // Security

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName))
                   .As<UserStorageSettings>()
                   .SingleInstance();

            builder.RegisterType<ApplicationUserStoreCache>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ApplicationUserStorePersistentStorage>()
                   .As<IApplicationUserStore>()
                   .SingleInstance();

            builder.RegisterType<ElasticSearchUserStorage>()
                   .AsSelf()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<PackageJsonConfigurationsInitializer>()
                   .As<IStartupInitializer>()
                   .SingleInstance();

            builder.RegisterType<DocumentIndexTypeInitializer>()
                   .As<IStartupInitializer>()
                   .SingleInstance();

            builder.RegisterType<SystemConfigApplicationEventHandler>()
                   .As<IApplicationEventHandler>()
                   .SingleInstance();

            // PrintView

            builder.RegisterType<PrintViewApi>()
                   .As<IPrintViewApi>()
                   .SingleInstance();

            // Registers

            builder.RegisterType<RegisterApi>()
                   .As<IRegisterApi>()
                   .SingleInstance();

            // Runtime

            builder.RegisterType<ActionUnitFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ScriptProcessor>()
                   .As<IScriptProcessor>()
                   .SingleInstance();

            // Services

            builder.RegisterType<DocumentTransactionScopeHttpGlobalHandler>().As<IHttpGlobalHandler>().SingleInstance();
            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}