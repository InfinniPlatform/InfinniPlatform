﻿using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory("C:\\Projects\\InfinniAspNet\\Assemblies");

            var host = new WebHostBuilder()
                    .UseUrls("http://localhost:9900")
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();

            host.Run();
        }
    }
}