using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Factories
{
	public interface IScriptFactory
	{
		/// <summary>
		/// Создает обработчик прикладных скриптов.
		/// </summary>
		IScriptProcessor BuildScriptProcessor();

		/// <summary>
		/// Создает провайдер метаданных прикладных скриптов.
		/// </summary>
		IScriptMetadataProvider BuildScriptMetadataProvider();
	}
}