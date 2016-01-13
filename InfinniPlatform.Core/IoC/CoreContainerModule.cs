using InfinniPlatform.Core.Compression;
using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.RestApi.Public;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.RestApi;
using InfinniPlatform.Sdk.Settings;

using PrintViewApi = InfinniPlatform.Core.RestApi.Public.PrintViewApi;
using RegisterApi = InfinniPlatform.Core.RestApi.Public.RegisterApi;

namespace InfinniPlatform.Core.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(AppConfiguration.Instance)
                   .As<IAppConfiguration>();

            builder.RegisterType<GZipDataCompressor>()
                   .As<IDataCompressor>()
                   .SingleInstance();

            builder.RegisterType<Log4NetLog>()
                   .As<ILog>()
                   .SingleInstance();

            builder.RegisterType<PerformanceLog>()
                   .As<IPerformanceLog>()
                   .SingleInstance();

            builder.RegisterType<LogComponent>()
                   .As<ILogComponent>()
                   .SingleInstance();

            // SaaS

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();

            // Transaction

            builder.RegisterType<DocumentTransactionScopeProvider>()
                   .As<IDocumentTransactionScopeProvider>()
                   .SingleInstance();

            // Log4Net
            builder.OnCreateInstance(new Log4NetContainerParameterResolver());
            builder.OnActivateInstance(new Log4NetContainerInstanceActivator());

            // DocumentApi

            builder.RegisterType<DocumentApi>()
                   .AsSelf()
                   .As<IDocumentApi>()
                   .SingleInstance();

            // PrintViewApi

            builder.RegisterType<PrintViewApi>()
                   .As<IPrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RestApi.DataApi.PrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            // RegisterApi

            builder.RegisterType<RegisterApi>()
                   .As<IRegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RestApi.DataApi.RegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RegistryComponent>()
                   .As<IRegistryComponent>()
                   .SingleInstance();

            // FileApi

            builder.RegisterType<FileApi>()
                   .As<IFileApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<UploadApi>()
                   .AsSelf()
                   .SingleInstance();

            // CustomServiceApi

            builder.RegisterType<CustomServiceApi>()
                   .As<ICustomServiceApi>()
                   .AsSelf()
                   .SingleInstance();

            // Fury

            builder.RegisterType<RestQueryApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<LocalQueryBuilder>()
                   .AsSelf()
                   .InstancePerDependency();
        }
    }
}