using System;

using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.NodeServiceHost
{
    public static class InfinniPlatformInprocessHost
    {
        // Mono очень не любит частое создание AppDomain и падает в этом случае с невообразимыми
        // исключениями. По этой причине данные класс единоразово (на процесс) создает экземпляр
        // платформы для ее тестирования.

        private static readonly InfinniPlatformServiceHostDomain ServerInstance = new InfinniPlatformServiceHostDomain();


        static InfinniPlatformInprocessHost()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => TryStop();
        }


        public static IDisposable Start()
        {
            TryStart();

            return FakeDisposableServer.Instance;
        }


        private static void TryStart()
        {
            try
            {
                ServerInstance.Start();

                ControllerRoutingFactory.Instance = new ControllerRoutingFactory(HostingConfig.Default);
            }
            catch
            {
                TryStop();

                throw;
            }
        }

        private static void TryStop()
        {
            try
            {
                ServerInstance.Stop();
            }
            catch
            {
            }

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