using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.Sdk.IoC;

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
        }
    }
}