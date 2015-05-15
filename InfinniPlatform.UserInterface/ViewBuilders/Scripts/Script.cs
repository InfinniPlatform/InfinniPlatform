namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
	/// <summary>
	/// Прикладной скрипт.
	/// </summary>
	public sealed class Script
	{
		/// <summary>
		/// Наименование функции.
		/// </summary>
		public string Name;

		/// <summary>
		/// Делегат для выполнения функции.
		/// </summary>
		public ScriptDelegate Action;
	}
}