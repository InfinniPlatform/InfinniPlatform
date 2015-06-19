namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
    public sealed class MetadataContainerValidationErrors : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.ValidationErrorsContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.ValidationError;
        }
    }
}