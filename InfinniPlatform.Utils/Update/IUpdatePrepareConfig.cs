using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Utils.Update
{
	public interface IUpdatePrepareConfig
	{
		HostingConfig HostingConfig { get; }

		string Version { get; }

		bool PrepareRoutingOperation();
	}
}