using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.WindowsServiceHost
{
    /// <summary>
    /// Обертка для запуска приложения как службы Windows.
    /// </summary>
    public class AspNetWin32Service : IWin32Service
    {
        private bool _stopRequestedByWindows;
        private readonly IWebHost _webHost;

        public AspNetWin32Service(string serviceName, IWebHost webHost)
        {
            ServiceName = serviceName;
            _webHost = webHost;
        }

        public string ServiceName { get; }

        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            _webHost.Services
                    .GetRequiredService<IApplicationLifetime>()
                    .ApplicationStopped
                    .Register(() =>
                    {
                        if (_stopRequestedByWindows == false)
                        {
                            serviceStoppedCallback();
                        }
                    });

            _webHost.Start();
        }

        public void Stop()
        {
            _stopRequestedByWindows = true;
            _webHost.Dispose();
        }
    }
}