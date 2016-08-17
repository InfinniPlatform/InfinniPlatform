using System;
using System.ComponentModel.Composition;
using System.Threading;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.IoC.Owin;
using InfinniPlatform.NodeServiceHost.Properties;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Hosting;

namespace InfinniPlatform.NodeServiceHost
{
    [Export("InfinniPlatformServiceHost")]
    public sealed class ServiceHost
    {
        private readonly object _statusSync = new object();
        private volatile ServiceHostStatus _serviceHostStatus = ServiceHostStatus.Stopped;
        private readonly Lazy<ServiceHostInstance> _serviceHostInstance = new Lazy<ServiceHostInstance>(CreateHostingService, LazyThreadSafetyMode.ExecutionAndPublication);


        public string Status => _serviceHostStatus.ToString();

        public IOwinHostingContext HostingContext => _serviceHostInstance.Value.OwinHostingContext;


        public void Start(TimeSpan timeout)
        {
            if (_serviceHostStatus != ServiceHostStatus.Running && _serviceHostStatus != ServiceHostStatus.StartPending)
            {
                lock (_statusSync)
                {
                    if (_serviceHostStatus != ServiceHostStatus.Running && _serviceHostStatus != ServiceHostStatus.StartPending)
                    {
                        var prevStatus = _serviceHostStatus;

                        _serviceHostStatus = ServiceHostStatus.StartPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStarting);

                            _serviceHostInstance.Value.HostingService.Start();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStarted);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStarted, error);

                            _serviceHostStatus = prevStatus;

                            throw;
                        }

                        _serviceHostStatus = ServiceHostStatus.Running;
                    }
                }
            }
        }

        public void Stop(TimeSpan timeout)
        {
            if (_serviceHostStatus != ServiceHostStatus.Stopped && _serviceHostStatus != ServiceHostStatus.StopPending)
            {
                lock (_statusSync)
                {
                    if (_serviceHostStatus != ServiceHostStatus.Stopped && _serviceHostStatus != ServiceHostStatus.StopPending)
                    {
                        var prevStatus = _serviceHostStatus;

                        _serviceHostStatus = ServiceHostStatus.StopPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStopping);

                            _serviceHostInstance.Value.HostingService.Stop();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStopped);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStopped, error);

                            _serviceHostStatus = prevStatus;

                            throw;
                        }

                        _serviceHostStatus = ServiceHostStatus.Stopped;
                    }
                }
            }
        }


        private static ServiceHostInstance CreateHostingService()
        {
            var owinHostingContext = GetOwinHostingContext();
            var owinHostingServiceFactory = new OwinHostingServiceFactory(owinHostingContext, Logger.Log);
            var owinHostingService = owinHostingServiceFactory.CreateHostingService();

            return new ServiceHostInstance(owinHostingService, owinHostingContext);
        }


        private static IOwinHostingContext GetOwinHostingContext()
        {
            // Поскольку в данном контексте IoC еще не доступен, настройки читаются напрямую
            var hostingConfig = AppConfiguration.Instance.GetSection<HostingConfig>(HostingConfig.SectionName);

            var owinHostingContextFactory = new AutofacOwinHostingContextFactory();
            var owinHostingContext = owinHostingContextFactory.CreateOwinHostingContext(hostingConfig);

            return owinHostingContext;
        }
    }
}