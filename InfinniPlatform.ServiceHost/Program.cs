using System;
using System.Globalization;
using System.Threading;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.ServiceHost.Properties;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US");


        public static void Main()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = DefaultCulture;
                Thread.CurrentThread.CurrentUICulture = DefaultCulture;

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