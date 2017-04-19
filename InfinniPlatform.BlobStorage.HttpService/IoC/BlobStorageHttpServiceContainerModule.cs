using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    public class BlobStorageHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<BlobStorageHttpService>().As<IHttpService>().SingleInstance();
        }
    }
}