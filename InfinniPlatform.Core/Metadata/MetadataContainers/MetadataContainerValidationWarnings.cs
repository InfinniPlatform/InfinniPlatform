namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    public sealed class MetadataContainerValidationWarnings : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.ValidationWarningsContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.ValidationWarning;
        }
    }
}