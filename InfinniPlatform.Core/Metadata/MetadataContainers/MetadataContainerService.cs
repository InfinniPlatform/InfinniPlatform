namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    internal sealed class MetadataContainerService : IMetadataContainerInfo
    {
        /// <summary>
        ///     Получить наименование контейнера метаданных
        /// </summary>
        /// <returns>Наименование контейнера метаданных</returns>
        public string GetMetadataContainerName()
        {
            return MetadataType.ServiceContainer;
        }

        /// <summary>
        ///     Получить наименование типа метаданных
        /// </summary>
        /// <returns>Наименование типа метаданных</returns>
        public string GetMetadataTypeName()
        {
            return MetadataType.Service;
        }
    }
}