namespace InfinniPlatform.Runtime
{
	/// <summary>
	/// Провайдер метаданных прикладных скриптов.
	/// </summary>
	public interface IScriptMetadataProvider
	{
		/// <summary>
		/// Возвращает метаданные прикладного скрипта.
		/// </summary>
		/// <param name="scriptIdentifier">Идентификатор прикладного скрипта.</param>
		ScriptMetadata GetScriptMetadata(string scriptIdentifier);

		/// <summary>
		/// Устанавливает метаданные прикладного скрипта.
		/// </summary>
		/// <param name="scriptMetadata">Метаданные прикладного скрипта.</param>
		void SetScriptMetadata(ScriptMetadata scriptMetadata);
	}
}