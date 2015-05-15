using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Runtime.Implementation.ScriptMetadataProviders
{
	/// <summary>
	///   Провайдер метаданных, хранящихся в памяти
	/// </summary>
	public sealed class ScriptMetadataProviderMemory : IScriptMetadataProvider
	{
		private readonly IList<ScriptMetadata> _scriptMetadataList = new List<ScriptMetadata>();

		/// <summary>
		///   Получить метаданные скриптов 
		/// </summary>
		/// <param name="scriptIdentifier">Идентификатор метаданных скрипта</param>
		/// <returns>Метаданные скрипта</returns>
		public ScriptMetadata GetScriptMetadata(string scriptIdentifier)
		{
			return _scriptMetadataList.FirstOrDefault(sc => sc.Identifier.ToLowerInvariant() == scriptIdentifier.ToLowerInvariant());
		}

		/// <summary>
		///   Добавить метаданные скрипта
		/// </summary>
		/// <param name="scriptMetadata">Метаданные скрипта</param>
		public void SetScriptMetadata(ScriptMetadata scriptMetadata)
		{
			_scriptMetadataList.Add(scriptMetadata);
		}
	}
}