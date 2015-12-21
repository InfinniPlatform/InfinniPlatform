using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Modules;
using InfinniPlatform.RestfulApi.Executors;
using InfinniPlatform.RestfulApi.Installers;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.RestfulApi.IoC
{
    internal sealed class RestfulApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<RestfulApiInstaller>()
                   .As<IModuleInstaller>()
                   .SingleInstance();

            builder.RegisterType<DocumentLinkMap>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ReferenceResolver>()
                   .As<IReferenceResolver>()
                   .SingleInstance();

            builder.RegisterType<SetDocumentExecutor>()
                   .As<ISetDocumentExecutor>()
                   .SingleInstance();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}