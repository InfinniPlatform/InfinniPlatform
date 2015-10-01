using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Threading;
using InfinniPlatform.Sdk.Api;


namespace InfinniPlatform.Api.TestEnvironment
{
    /// <summary>
    ///     Информация о тестовом сервере.
    /// </summary>
    internal sealed class TestServerInfo
    {
        public HostingConfig HostingConfig;
        public ProcessDispatcher ServerProcess;
    }
}