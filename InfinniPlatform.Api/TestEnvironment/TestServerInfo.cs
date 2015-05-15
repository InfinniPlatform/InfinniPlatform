using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Threading;

namespace InfinniPlatform.Api.TestEnvironment
{
	/// <summary>
	/// Информация о тестовом сервере.
	/// </summary>
	sealed class TestServerInfo
	{
		public ProcessDispatcher ServerProcess;
		public HostingConfig HostingConfig;
	}
}