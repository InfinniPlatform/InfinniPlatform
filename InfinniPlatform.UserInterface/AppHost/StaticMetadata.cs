using InfinniPlatform.UserInterface.Configurations;

namespace InfinniPlatform.UserInterface.AppHost
{
    /// <summary>
    ///     Статические метаданные приложения.
    /// </summary>
    internal static class StaticMetadata
    {      
        /// <summary>
        ///     Создать метаданные главного окна приложения.
        /// </summary>
        public static dynamic CreateAppView(string server, int port, string routeVersion)
        {
			var viewMetadataService = ConfigResourceRepository.GetView("Designer", "Common", "App");
	        return viewMetadataService;
        }

        /// <summary>
        ///     Сгенерировать версию конфигурации
        /// </summary>
        /// <returns></returns>
        public static string CreateVersion()
        {
            return "1.0.0.0"; //Guid.NewGuid().ToString();
        }
    }
}