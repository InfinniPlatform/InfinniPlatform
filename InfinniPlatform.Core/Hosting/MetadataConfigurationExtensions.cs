using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    ///     Расширения для конфигурации метаданных
    /// </summary>
    public static class MetadataConfigurationExtensions
    {
        /// <summary>
        ///     Получить идентификатор метаданных точки расширения бизнес-логики
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        /// <param name="configRequestProvider">Провайдер роутинга запроса</param>
        /// <param name="extensionPointTypeName">Тип точки расширения</param>
        /// <returns>Идентификатор точки расширения логики</returns>
        public static string GetExtensionPointValue(this IMetadataConfiguration metadataConfiguration,
            IConfigRequestProvider configRequestProvider, string extensionPointTypeName)
        {
            return metadataConfiguration.GetExtensionPointValue(configRequestProvider.GetMetadataIdentifier(),
                configRequestProvider.GetServiceName(), extensionPointTypeName);
        }
    }
}