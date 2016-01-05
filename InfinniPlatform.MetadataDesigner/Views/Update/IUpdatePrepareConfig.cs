using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.MetadataDesigner.Views.Update
{
	public interface IUpdatePrepareConfig
	{
		HostingConfig HostingConfig { get; }

		string Version { get; }

		bool PrepareRoutingOperation();
	}
}