using System;
using System.Threading;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.ServiceHost.Properties;
using InfinniPlatform.WebApi.Logging;

using Topshelf;

namespace InfinniPlatform.ServiceHost
{
	class Program
	{
		static void Main()
		{
			AppDomain.CurrentDomain.SetupLogErrorHandler();

			var consoleMode = AppSettings.GetValue("ConsoleMode", true);
			var serviceName = AppSettings.GetValue("ApplicationServiceName", "InfinniPlatform");
			var serviceDescription = AppSettings.GetValue("ApplicationServiceDescription", "InfinniPlatform");

			var serviceHost = new InfinniPlatformServiceHost();

			if (consoleMode)
			{
				try
				{
					serviceHost.Start(Timeout.InfiniteTimeSpan);

					Console.WriteLine(Resources.ServerStartedAt, string.Format("{0}:{1}", AppSettings.GetValue("AppServerName"), AppSettings.GetValue("AppServerPort")));
					Console.ReadLine();
				}
				finally
				{
					serviceHost.Stop(Timeout.InfiniteTimeSpan);
				}
			}
			else
			{
				HostFactory.Run(x =>
								{
									x.Service<InfinniPlatformServiceHost>(config =>
																		  {
																			  config.ConstructUsing(hostSettings => serviceHost);
																			  config.WhenStarted(s => s.Start(Timeout.InfiniteTimeSpan));
																			  config.WhenStopped(s => s.Stop(Timeout.InfiniteTimeSpan));
																		  });

									x.SetServiceName(serviceName);
									x.SetDisplayName(serviceName);
									x.SetDescription(serviceDescription);
								});
			}
		}
	}
}