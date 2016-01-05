namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    public sealed class MetadataContainerDocument : IMetadataContainerInfo
    {
        /// <summary>
        ///     Получить наименование контейнера метаданных
        /// </summary>
        /// <returns>Наименование контейнера метаданных</returns>
        public string GetMetadataContainerName()
        {
            return MetadataType.DocumentContainer;
        }

        /// <summary>
        ///     Получить наименование типа метаданных
        /// </summary>
        /// <returns>Наименование типа метаданных</returns>
        public string GetMetadataTypeName()
        {
            return MetadataType.Document;
        }
    }
}