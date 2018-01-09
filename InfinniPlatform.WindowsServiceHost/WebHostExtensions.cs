using DasMulli.Win32.ServiceUtils;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for <see cref="IWebHost"/>.
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Starts an application in Windows service wrappper.
        /// </summary>
        /// <param name="webHost">Web host instance.</param>
        public static void RunAsService(this IWebHost webHost)
        {
            var aspNetWin32Service = new AspNetWin32Service(nameof(AspNetWin32Service), webHost);
            var serviceHost = new Win32ServiceHost(aspNetWin32Service);

            serviceHost.Run();
        }
    }
}