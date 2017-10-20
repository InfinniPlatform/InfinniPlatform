using DasMulli.Win32.ServiceUtils;
using InfinniPlatform.WindowsServiceHost;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace InfinniPlatform.AspNetCore
{
    public static class WebHostExtensions
    {
        public static void RunAsService(this IWebHost webHost)
        {
            var aspNetWin32Service = new AspNetWin32Service(nameof(AspNetWin32Service), webHost);
            var serviceHost = new Win32ServiceHost(aspNetWin32Service);

            serviceHost.Run();
        }
    }
}