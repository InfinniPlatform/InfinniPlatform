using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.SystemConfig.Metadata;
using InfinniPlatform.SystemConfig.PrintView;
using InfinniPlatform.SystemConfig.Registers;
using InfinniPlatform.SystemConfig.Runtime;
using InfinniPlatform.SystemConfig.Services;
using InfinniPlatform.SystemConfig.StartupInitializers;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class SystemConfigContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Metadata

            builder.RegisterType<MetadataApi>()
                   .As<IMetadataApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ElasticTypesMigrationHelper>()
                   .AsSelf()
                   .SingleInstance();

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

            // Hosting

            builder.RegisterType<PackageJsonConfigurationsInitializer>()
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