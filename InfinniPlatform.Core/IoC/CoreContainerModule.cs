using InfinniPlatform.Compression;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.Global;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemInfo;
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

            builder.RegisterType<SystemInfoProvider>()
                   .As<ISystemInfoProvider>()
                   .SingleInstance();

            builder.RegisterType<ScriptContext>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<TransactionManager>()
                   .As<ITransactionManager>()
                   .SingleInstance();

            builder.RegisterType<TransactionComponent>()
                   .As<ITransactionComponent>()
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

            builder.RegisterType<LogComponent>()
                   .As<ILogComponent>()
                   .SingleInstance();

            builder.RegisterType<ProfilerComponent>()
                   .As<IProfilerComponent>()
                   .AsSelf()
                   .SingleInstance();

            // Log4Net
            builder.OnCreateInstance(new Log4NetContainerParameterResolver());
            builder.OnActivateInstance(new Log4NetContainerInstanceActivator());

            // DocumentApi

            builder.RegisterType<Api.RestApi.Public.DocumentApi>()
                   .As<Sdk.ApiContracts.IDocumentApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.DataApi.DocumentApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.DataApi.DocumentApiUnsecured>()
                   .AsSelf()
                   .SingleInstance();

            // PrintViewApi

            builder.RegisterType<Api.RestApi.Public.PrintViewApi>()
                   .As<Sdk.ApiContracts.IPrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.DataApi.PrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            // RegisterApi

            builder.RegisterType<Api.RestApi.Public.RegisterApi>()
                   .As<Sdk.ApiContracts.IRegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.DataApi.RegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RegistryComponent>()
                   .As<IRegistryComponent>()
                   .SingleInstance();

            // FileApi

            builder.RegisterType<Api.RestApi.Public.FileApi>()
                   .As<Sdk.ApiContracts.IFileApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.DataApi.UploadApi>()
                   .AsSelf()
                   .SingleInstance();

            // AuthApi

            builder.RegisterType<Api.RestApi.Public.AuthApi>()
                   .As<Sdk.ApiContracts.IAuthApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.Auth.AuthApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<Api.RestApi.Auth.SignInApi>()
                   .AsSelf()
                   .SingleInstance();

            // CustomServiceApi

            builder.RegisterType<Api.RestApi.Public.CustomServiceApi>()
                   .As<Sdk.ApiContracts.ICustomServiceApi>()
                   .AsSelf()
                   .SingleInstance();

            // ХЗ

            builder.RegisterType<InprocessDocumentComponent>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}