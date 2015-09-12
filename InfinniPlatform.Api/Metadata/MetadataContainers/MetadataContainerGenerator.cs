namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
    public sealed class MetadataContainerGenerator : IMetadataContainerInfo
    {
        /// <summary>
        ///     Получить наименование контейнера метаданных
        /// </summary>
        /// <returns>Наименование контейнера метаданных</returns>
        public string GetMetadataContainerName()
        {
            return MetadataType.GeneratorContainer;
        }

        /// <summary>
        ///     Получить наименование типа метаданных
        /// </summary>
        /// <returns>Наименование типа метаданных</returns>
        public string GetMetadataTypeName()
        {
            return MetadataType.Generator;
        }
    }
}