namespace InfinniPlatform.Runtime
{
    /// <summary>
    ///     Загрузчик версий в кэш объектов
    /// </summary>
    public interface IVersionLoader
    {
        /// <summary>
        ///     Загрузить список версий в кэш объектов
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <returns>Список загруженных кэшей</returns>
        MethodInvokationCacheList ConstructInvokationCache(string version, string metadataConfigurationId);

        /// <summary>
        ///     Обновить указанную версию в кэще объектов
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <param name="versionCacheList">Обновляемый кэш версий конфигураций</param>
        void UpdateInvokationCache(string version, string metadataConfigurationId,
            MethodInvokationCacheList versionCacheList);
    }
}