using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    public class BlobStorageHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<BlobStorageHttpService>().As<IHttpService>().SingleInstance();
            builder.RegisterType<BlobStorageService>().As<IBlobStorageService>().SingleInstance();
        }
    }
}