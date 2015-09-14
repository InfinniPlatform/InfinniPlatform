namespace InfinniPlatform.Api.Metadata.MetadataContainers
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