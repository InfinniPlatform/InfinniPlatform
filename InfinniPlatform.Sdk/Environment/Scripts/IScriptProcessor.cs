namespace InfinniPlatform.Sdk.Environment.Scripts
{
	/// <summary>
	/// Обработчик прикладных скриптов.
	/// </summary>
	public interface IScriptProcessor
	{
		/// <summary>
		/// Выполняет прикладной скрипт.
		/// </summary>
		/// <param name="scriptIdentifier">Идентификатор прикладного скрипта.</param>
		/// <param name="scriptContext">Контекст прикладного скрипта.</param>
		/// <returns>Результат работы прикладного скрипта.</returns>
		object InvokeScript(string scriptIdentifier, object scriptContext);
	}
}