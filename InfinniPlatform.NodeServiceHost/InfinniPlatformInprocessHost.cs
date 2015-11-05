using System;
using System.Net.Sockets;
using System.Reflection;

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
            AppDomain.CurrentDomain.DomainUnload += (s, e) => TryStop();
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
            catch (TargetInvocationException e)
            {
                // Почему-то при запуске тестов в Mono иногда происходит SocketError.AddressAlreadyInUse,
                // причем никакие виды синхронизаций не помогают. Даже после гарантированной остановки
                // службы ошибка проявляется. Помогло только игнорирование этой ошибки. Конечно, все это
                // выглядит довольно отвратительно, однако данный код был оставлен лишь по той причине,
                // что большая часть тестов будет переделываться в формат легковесных Unit-тестов.

                var socketException = e.InnerException as SocketException;

                if (socketException == null || socketException.SocketErrorCode != SocketError.AddressAlreadyInUse)
                {
                    throw;
                }
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