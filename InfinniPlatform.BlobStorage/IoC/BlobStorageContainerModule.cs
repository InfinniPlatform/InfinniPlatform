using InfinniPlatform.BlobStorage.Contract;
using InfinniPlatform.BlobStorage.Services;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.BlobStorage.IoC
{
    internal sealed class BlobStorageContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<FileSystemBlobStorageSettings>(FileSystemBlobStorageSettings.SectionName))
                   .As<FileSystemBlobStorageSettings>()
                   .SingleInstance();

            builder.RegisterType<FileSystemBlobStorage>()
                   .As<IBlobStorage>()
                   .SingleInstance();

            builder.RegisterType<BlobHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();
        }
    }
}