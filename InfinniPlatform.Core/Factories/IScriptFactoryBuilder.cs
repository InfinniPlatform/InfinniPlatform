namespace InfinniPlatform.Factories
{
	/// <summary>
	/// Конструктор фабрик для создания классов по работе с прикладными скриптами.
	/// </summary>
	public interface IScriptFactoryBuilder
	{
		/// <summary>
		/// Создает фабрику для создания классов по работе с прикладными скриптами.
		/// </summary>
		IScriptFactory BuildScriptFactory(string metadataConfigurationId);
	}
}