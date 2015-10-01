using System;
using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ScriptMetadataProviders
{
	/// <summary>
	///   Провайдер метаданных, хранящихся в памяти
	/// </summary>
	public sealed class ScriptMetadataProviderMemory : IScriptMetadataProvider
	{
		private readonly Dictionary<string, ScriptMetadata> _scriptMetadataList
			= new Dictionary<string, ScriptMetadata>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		///   Получить метаданные скриптов 
		/// </summary>
		/// <param name="scriptIdentifier">Идентификатор метаданных скрипта</param>
		/// <returns>Метаданные скрипта</returns>
		public ScriptMetadata GetScriptMetadata(string scriptIdentifier)
		{
			ScriptMetadata scriptMetadata;

			_scriptMetadataList.TryGetValue(scriptIdentifier, out scriptMetadata);

			return scriptMetadata;
		}

		/// <summary>
		///   Добавить метаданные скрипта
		/// </summary>
		/// <param name="scriptMetadata">Метаданные скрипта</param>
		public void SetScriptMetadata(ScriptMetadata scriptMetadata)
		{
			var scriptIdentifier = scriptMetadata.Identifier;

			_scriptMetadataList[scriptIdentifier] = scriptMetadata;
		}
	}
}