using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.Executors;
using InfinniPlatform.SystemConfig.Installers;
using InfinniPlatform.SystemConfig.SystemInfo;
using InfinniPlatform.SystemConfig.Utils;
using InfinniPlatform.SystemInfo;
using InfinniPlatform.Transactions;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class RestfulApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SystemInfoProvider>()
                   .As<ISystemInfoProvider>()
                   .SingleInstance();

            builder.RegisterType<RestfulApiInstaller>()
                   .As<IModuleInstaller>()
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

            builder.RegisterType<DocumentExecutor>()
                   .AsSelf()
                   .InstancePerDependency();

            // UserStorage

            builder.RegisterType<ElasticSearchUserStorage>()
                   .AsSelf()
                   .SingleInstance();

            // TransactionScope

            builder.RegisterType<ElasticDocumentTransactionScope>()
                   .As<IDocumentTransactionScope>()
                   .InstancePerRequest();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}