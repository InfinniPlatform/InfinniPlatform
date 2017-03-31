using Microsoft.AspNetCore.Hosting;

namespace InfinniPlatform.ServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                    .UseUrls("http://localhost:9900")
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();

            host.Run();
        }
    }
}