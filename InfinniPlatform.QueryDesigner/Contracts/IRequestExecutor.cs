using InfinniPlatform.Sdk.Hosting;

namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IRequestExecutor
    {
        void InitRouting(HostingConfig hostingConfig);
    }
}