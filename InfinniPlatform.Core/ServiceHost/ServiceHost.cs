using System;
using System.ComponentModel.Composition;
using System.Threading;

using InfinniPlatform.Core.Http.Hosting;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.ServiceHost
{
    [Export("InfinniPlatformServiceHost")]
    public class ServiceHost
    {
        public ServiceHost()
        {
            _statusSync = new object();
            _status = ServiceHostStatus.Stopped;

            _hostingService = new Lazy<IHostingService>(CreateHostingService, LazyThreadSafetyMode.ExecutionAndPublication);
            _containerResolver = new Lazy<IContainerResolver>(GetContainerResolver, LazyThreadSafetyMode.ExecutionAndPublication);
        }


        private readonly object _statusSync;
        private volatile ServiceHostStatus _status;

        private readonly Lazy<IHostingService> _hostingService;
        private readonly Lazy<IContainerResolver> _containerResolver;


        public object Status => _status;

        public IContainerResolver ContainerResolver => _containerResolver.Value;


        public void Init(TimeSpan timeout)
        {
            if ((_status != ServiceHostStatus.Initialized) && (_status != ServiceHostStatus.InitializePending))
            {
                lock (_statusSync)
                {
                    if ((_status != ServiceHostStatus.Initialized) && (_status != ServiceHostStatus.InitializePending))
                    {
                        var prevStatus = _status;

                        _status = ServiceHostStatus.InitializePending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostInitializationWasStarted);

                            _hostingService.Value.Init();

                            Logger.Log.Info(Resources.ServiceHostInitializationSuccessfullyCompleted);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostInitializationCompletedWithAnException, error);

                            _status = prevStatus;

                            throw;
                        }

                        _status = ServiceHostStatus.Initialized;
                    }
                }
            }
        }

        public void Start(TimeSpan timeout)
        {
            if (_status != ServiceHostStatus.Started && _status != ServiceHostStatus.StartPending)
            {
                lock (_statusSync)
                {
                    if (_status != ServiceHostStatus.Started && _status != ServiceHostStatus.StartPending)
                    {
                        var prevStatus = _status;

                        _status = ServiceHostStatus.StartPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStarting);

                            _hostingService.Value.Start();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStarted);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStarted, error);

                            _status = prevStatus;

                            throw;
                        }

                        _status = ServiceHostStatus.Started;
                    }
                }
            }
        }

        public void Stop(TimeSpan timeout)
        {
            if (_status != ServiceHostStatus.Stopped && _status != ServiceHostStatus.StopPending)
            {
                lock (_statusSync)
                {
                    if (_status != ServiceHostStatus.Stopped && _status != ServiceHostStatus.StopPending)
                    {
                        var prevStatus = _status;

                        _status = ServiceHostStatus.StopPending;

                        try
                        {
                            Logger.Log.Info(Resources.ServiceHostIsStopping);

                            _hostingService.Value.Stop();

                            Logger.Log.Info(Resources.ServiceHostHasBeenSuccessfullyStopped);
                        }
                        catch (Exception error)
                        {
                            Logger.Log.Fatal(Resources.ServiceHostHasNotBeenStopped, error);

                            _status = prevStatus;

                            throw;
                        }

                        _status = ServiceHostStatus.Stopped;
                    }
                }
            }
        }


        private IHostingService CreateHostingService()
        {
            var containerResolver = _containerResolver.Value;

            return containerResolver.Resolve<IHostingService>();
        }

        private static IContainerResolver GetContainerResolver()
        {
            var containerResolverFactory = new AutofacHttpContainerResolverFactory();
            var containerResolver = containerResolverFactory.CreateContainerResolver();

            return containerResolver;
        }
    }
}