using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform
{
	/// <summary>
	/// Предоставляет настройки окружения для тестов.
	/// </summary>
	public static class TestSettings
	{
		/// <summary>
		/// Настройки подсистемы хостинга.
		/// </summary>
		public static readonly HostingConfig DefaultHostingConfig = new HostingConfig();
	}
}