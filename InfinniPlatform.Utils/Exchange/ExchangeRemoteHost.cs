using System;

using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Utils.Update;

namespace InfinniPlatform.Utils.Exchange
{
	public sealed class ExchangeRemoteHost : IUpdatePrepareConfig
	{
		public ExchangeRemoteHost(HostingConfig hostingConfig, string version)
		{
			_version = version;
			HostingConfig = hostingConfig;
		}

		readonly string _version;

		public HostingConfig HostingConfig { get; private set; }

		public string Version
		{
			get { return _version; }
		}

		public bool PrepareRoutingOperation()
		{
			if (string.IsNullOrEmpty(HostingConfig.ServerScheme) || string.IsNullOrEmpty(HostingConfig.ServerName) || HostingConfig.ServerPort <= 0)
			{
				Console.WriteLine(@"Не указана схема, сервер или порт для обновления. Обновить конфигурацию локально?");
				if (Console.ReadKey().KeyChar != 'y')
				{
					return false;
				}

				HostingConfig = HostingConfig.Default;
			}

			Api.RestQuery.ControllerRoutingFactory.Instance = new Api.RestQuery.ControllerRoutingFactory(HostingConfig);

			return true;
		}
	}
}