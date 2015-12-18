using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    internal sealed class BlobStorageContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<FileSystemBlobStorageFactory>()
                   .As<IBlobStorageFactory>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IBlobStorageFactory>().CreateBlobStorage())
                   .As<IBlobStorage>()
                   .SingleInstance();

            builder.RegisterType<BlobStorageComponent>()
                   .As<IBlobStorageComponent>()
                   .SingleInstance();
        }
    }
}