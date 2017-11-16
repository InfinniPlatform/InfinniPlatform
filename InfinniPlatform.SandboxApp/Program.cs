using System;
using System.IO;
using InfinniPlatform.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace InfinniPlatform.SandboxApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogger();

            try
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("AppConfig.json", true, true)
                              .AddEnvironmentVariables();

                        if (args != null)
                        {
                            config.AddCommandLine(args);
                        }
                    })
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogger()
        {
            const string outputTemplate = "{Timestamp:o}|{Level:u3}|{RequestId}|{UserName}|{SourceContext}|{Message}{NewLine}{Exception}";
            const string outputTemplatePerf = "{Timestamp:o}|{RequestId}|{UserName}|{SourceContext}|{Message}{NewLine}";

            var performanceLoggerFilter = Matching.WithProperty<string>(Constants.SourceContextPropertyName,
                                                                        p => p.StartsWith(nameof(IPerformanceLogger)));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => lc.Filter.ByExcluding(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/events-{Date}.log",
                                                             outputTemplate: outputTemplate)
                                        .WriteTo.LiterateConsole(outputTemplate: outputTemplate))
                .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/performance-{Date}.log",
                                                             outputTemplate: outputTemplatePerf))
                .CreateLogger();
        }
    }
}