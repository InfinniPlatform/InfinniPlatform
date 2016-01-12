using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IRequestExecutor
    {
        void InitRouting(HostingConfig hostingConfig);
    }
}