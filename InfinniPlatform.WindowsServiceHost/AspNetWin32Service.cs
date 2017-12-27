using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform
{
    /// <summary>
    /// Wrapper for hosting application as Windows service.
    /// </summary>
    public class AspNetWin32Service : IWin32Service
    {
        private bool _stopRequestedByWindows;
        private readonly IWebHost _webHost;

        /// <summary>
        /// Initializes a new instance of <see cref="AspNetWin32Service" />.
        /// </summary>
        /// <param name="serviceName">Windows service name.</param>
        /// <param name="webHost">Web host instance.</param>
        public AspNetWin32Service(string serviceName, IWebHost webHost)
        {
            ServiceName = serviceName;
            _webHost = webHost;
        }

        /// <inheritdoc />
        public string ServiceName { get; }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Stop()
        {
            _stopRequestedByWindows = true;
            _webHost.Dispose();
        }
    }
}