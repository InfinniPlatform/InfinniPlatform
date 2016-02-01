using System;
using System.ComponentModel.Composition;
using System.Threading;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.IoC.Owin;
using InfinniPlatform.NodeServiceHost.Properties;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Hosting;

namespace InfinniPlatform.NodeServiceHost
{
    [Export("Infinni.NodeWorker.ServiceHost.IWorkerServiceHost")]
    public sealed class InfinniPlatformServiceHost
    {
        private volatile Status _status = Status.Stopped;
        private readonly object _statusSync = new object();
        private readonly Lazy<IHostingService> _hostingService = new Lazy<IHostingService>(CreateHostingService, LazyThreadSafetyMode.ExecutionAndPublication);


        public string GetStatus()
        {
            return _status.ToString();
        }

        public void Start(TimeSpan timeout)
        {
            if (_status != Status.Running && _status != Status.StartPending)
            {
                lock (_statusSync)
                {
                    if (_status != Status.Running && _status != Status.StartPending)
                    {
                        var prevStatus = _status;

                        _status = Status.StartPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStarting);

                            _hostingService.Value.Start();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStarted);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStarted, null, error);

                            _status = prevStatus;

                            throw;
                        }

                        _status = Status.Running;
                    }
                }
            }
        }

        public void Stop(TimeSpan timeout)
        {
            if (_status != Status.Stopped && _status != Status.StopPending)
            {
                lock (_statusSync)
                {
                    if (_status != Status.Stopped && _status != Status.StopPending)
                    {
                        var prevStatus = _status;

                        _status = Status.StopPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStopping);

                            _hostingService.Value.Stop();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStopped);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStopped, null, error);

                            _status = prevStatus;

                            throw;
                        }

                        _status = Status.Stopped;
                    }
                }
            }
        }


        private static IHostingService CreateHostingService()
        {
            var owinHostingContext = GetOwinHostingContext();
            var owinHostingServiceFactory = new OwinHostingServiceFactory(owinHostingContext);
            var owinHostingService = owinHostingServiceFactory.CreateHostingService();

            return owinHostingService;
        }


        public static IOwinHostingContext GetOwinHostingContext()
        {
            // Поскольку в данном контексте IoC еще не доступен, настройки читаются напрямую
            var hostingConfig = AppConfiguration.Instance.GetSection<HostingConfig>(HostingConfig.SectionName);

            var owinHostingContextFactory = new AutofacOwinHostingContextFactory();
            var owinHostingContext = owinHostingContextFactory.CreateOwinHostingContext(hostingConfig);

            return owinHostingContext;
        }


        public enum Status
        {
            Stopped,
            StartPending,
            Running,
            StopPending
        }
    }
}