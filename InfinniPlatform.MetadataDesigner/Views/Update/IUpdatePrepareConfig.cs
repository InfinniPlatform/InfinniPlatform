using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.MetadataDesigner.Views.Update
{
	public interface IUpdatePrepareConfig
	{
		HostingConfig HostingConfig { get; }

		string Version { get; }

		bool PrepareRoutingOperation();
	}
}