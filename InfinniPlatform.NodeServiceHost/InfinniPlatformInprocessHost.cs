using System;

using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.NodeServiceHost
{
    public static class InfinniPlatformInprocessHost
    {
        private static readonly InfinniPlatformServiceHostDomain ServerInstance = new InfinniPlatformServiceHostDomain();


        public static IDisposable Start()
        {
            try
            {
                ServerInstance.Start();

                ControllerRoutingFactory.Instance = new ControllerRoutingFactory(HostingConfig.Default);
            }
            catch
            {
                try
                {
                    ServerInstance.Stop();
                }
                catch
                {
                }

                throw;
            }

            return FakeDisposableServer.Instance;
        }


        private class FakeDisposableServer : IDisposable
        {
            public static readonly FakeDisposableServer Instance = new FakeDisposableServer();

            public void Dispose()
            {
            }
        }
    }
}