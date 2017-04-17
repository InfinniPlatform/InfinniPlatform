using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.BlobStorage.HttpService.IoC
{
    public class BlobStorageHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<BlobStorageHttpService>().As<IHttpService>().SingleInstance();
        }
    }
}