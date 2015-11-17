namespace InfinniPlatform.Runtime
{
	/// <summary>
	/// Загрузчик версий прикладных скриптов.
	/// </summary>
	public interface IVersionLoader
	{
	    /// <summary>
	    /// Конструирует кэш прикладных скриптов для заданной версии указанной конфигурации.
	    /// </summary>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации.</param>
	    /// <returns>Список кэшей прикладный скриптов конфигурации для вызова точек расширения бизнес-логики.</returns>
	    IMethodInvokationCacheList ConstructInvokationCache(string metadataConfigurationId);

	    /// <summary>
	    /// Обновляет кэш прикладных скриптов для заданной версии указанной конфигурации.
	    /// </summary>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации.</param>
	    /// <param name="versionCacheList">Список кэшей прикладный скриптов конфигурации для вызова точек расширения бизнес-логики.</param>
	    void UpdateInvokationCache(string metadataConfigurationId, IMethodInvokationCacheList versionCacheList);
	}
}