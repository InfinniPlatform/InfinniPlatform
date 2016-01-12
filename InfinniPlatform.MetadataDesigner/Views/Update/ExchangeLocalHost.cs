using InfinniPlatform.Sdk.RestApi;

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
			return true;
		}
	}
}