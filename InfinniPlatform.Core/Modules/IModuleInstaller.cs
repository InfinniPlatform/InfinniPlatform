using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Modules
{
    /// <summary>
    ///   Модуль установки сервисов конфигурации
    /// </summary>
	public interface IModuleInstaller
    {
		/// <summary>
		///  Установить модуль приложения
		/// </summary>
	    IModule InstallModule();

		/// <summary>
		///   Получить наименование модуля приложения
		/// </summary>
	    string ModuleName { get;  }

        /// <summary>
        ///  Является ли модуль системным (стартует перед остальными, не может ссылаться на другие системные модули)
        /// </summary>
        bool IsSystem { get; }
	}
}
