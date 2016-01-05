using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IRequestExecutor
    {
        void InitRouting(HostingConfig hostingConfig);
    }
}