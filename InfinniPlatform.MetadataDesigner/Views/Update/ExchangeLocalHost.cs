using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.TestEnvironment;
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

			TestApi.InitClientRouting(HostingConfig.Default);

			return true;
		}
	}
}