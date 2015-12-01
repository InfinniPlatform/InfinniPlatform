using System;
using System.Threading;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    class Program
    {
        static void Main()
        {
            var serviceHost = new InfinniPlatformServiceHost();

            try
            {
                serviceHost.Start(Timeout.InfiniteTimeSpan);
                Console.WriteLine(Resources.ServerStarted);
                Console.ReadLine();
            }
            finally
            {
                serviceHost.Stop(Timeout.InfiniteTimeSpan);
            }
        }
    }
}