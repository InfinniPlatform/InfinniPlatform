using System;

using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.NodeServiceHost
{
	public static class InfinniPlatformInprocessHost
	{
		public static IDisposable Start()
		{
			var server = new InfinniPlatformServiceHostDomain();
			server.Start();

			ControllerRoutingFactory.Instance = new ControllerRoutingFactory(HostingConfig.Default);

			return server;
		}
	}
}