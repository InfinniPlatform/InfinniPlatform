using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;

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