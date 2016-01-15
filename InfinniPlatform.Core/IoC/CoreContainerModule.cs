using InfinniPlatform.Core.Compression;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

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
        }
    }
}