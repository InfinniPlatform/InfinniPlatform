namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    public sealed class MetadataContainerMenu : IMetadataContainerInfo
    {
        /// <summary>
        ///     Получить наименование контейнера метаданных
        /// </summary>
        /// <returns>Наименование контейнера метаданных</returns>
        public string GetMetadataContainerName()
        {
            return MetadataType.MenuContainer;
        }

        /// <summary>
        ///     Получить наименование типа метаданных
        /// </summary>
        /// <returns>Наименование типа метаданных</returns>
        public string GetMetadataTypeName()
        {
            return MetadataType.Menu;
        }
    }
}