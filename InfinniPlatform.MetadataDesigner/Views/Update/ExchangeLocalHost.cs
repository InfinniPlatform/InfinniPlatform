using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.MetadataDesigner.Views.Update
{
	sealed class ExchangeLocalHost : IUpdatePrepareConfig
	{
		public HostingConfig HostingConfig { get; private set; }

		public string Version { get; private set; }

		public bool PrepareRoutingOperation()
		{
			HostingConfig = HostingConfig.Default;
			Version = "TestVersion";

			ControllerRoutingFactory.Instance = new ControllerRoutingFactory(HostingConfig.Default);

			return true;
		}
	}
}