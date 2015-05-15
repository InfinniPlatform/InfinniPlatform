namespace InfinniPlatform.Runtime
{
	/// <summary>
	///   Загрузчик версий в кэш объектов
	/// </summary>
	public interface IVersionLoader
	{
		/// <summary>
		///  Загрузить список версий в кэш объектов
		/// </summary>
		/// <param name="metadataConfigurationId"></param>
		/// <returns>Список загруженных кэшей</returns>
		MethodInvokationCacheList ConstructInvokationCache(string metadataConfigurationId);
	}
}