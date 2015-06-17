using InfinniPlatform.Api.Factories;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.Factories
{
	/// <summary>
	///   Фабрика, конструирующая реализации исполнителей скриптов
	/// </summary>
	public interface IScriptFactory
	{
	    /// <summary>
	    ///   Создать процессор запуска прикладных скриптов на основе использования
	    ///   распределенного хранилища
	    /// </summary>
	    /// <returns>Процессор запуска прикладных скриптов</returns>
	    IScriptProcessor BuildScriptProcessor();

	    /// <summary>
	    ///   Создать провайдер метаданных прикладных скриптов
	    /// </summary>
	    /// <returns>Провайдер метаданных прикладных скриптов</returns>
	    IScriptMetadataProvider BuildScriptMetadataProvider();
	}
}