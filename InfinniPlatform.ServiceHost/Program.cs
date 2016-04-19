using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                InfinniPlatformInprocessHost.Start();

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