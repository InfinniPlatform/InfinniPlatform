using System;
using System.Threading;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main()
        {
            var infinniPlatformServiceHost = new InfinniPlatformServiceHost();

            try
            {
                infinniPlatformServiceHost.Start(Timeout.InfiniteTimeSpan);

                Console.WriteLine(Resources.ServerStarted);
                Console.ReadLine();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
    }
}