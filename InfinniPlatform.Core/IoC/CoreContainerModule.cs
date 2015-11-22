using InfinniPlatform.Compression;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.Global;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Transactions;

namespace InfinniPlatform.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<GlobalContext>()
                   .As<IGlobalContext>();

            builder.RegisterType<PlatformComponentsPack>()
                   .As<IPlatformComponentsPack>();

            builder.RegisterType<CustomServiceGlobalContext>()
                   .As<ICustomServiceGlobalContext>();

            builder.RegisterType<TransactionManager>()
                   .As<ITransactionManager>()
                   .SingleInstance();

            builder.RegisterType<GZipDataCompressor>()
                   .As<IDataCompressor>()
                   .SingleInstance();

            builder.RegisterType<Log4NetLog>()
                   .As<ILog>()
                   .SingleInstance();

            builder.RegisterType<PerformanceLog>()
                   .As<IPerformanceLog>()
                   .SingleInstance();

            builder.OnCreateInstance(new Log4NetContainerParameterResolver());
            builder.OnActivateInstance(new Log4NetContainerInstanceActivator());
        }
    }
}