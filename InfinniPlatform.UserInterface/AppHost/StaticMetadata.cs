using InfinniPlatform.Api.Settings;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface.AppHost
{
	/// <summary>
	/// Статические метаданные приложения.
	/// </summary>
	static class StaticMetadata
	{
		/// <summary>
		/// Создать метаданные главного окна приложения.
		/// </summary>
		public static dynamic CreateAppView()
		{
			var configId = AppSettings.GetValue("ConfigId");
			var viewMetadataService = new ViewMetadataService(null, configId, "Common");
			return viewMetadataService.GetItem("App");
		}
	}
}