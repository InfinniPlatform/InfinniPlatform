namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    public sealed class MetadataContainerStatus : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.StatusContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.Status;
        }
    }
}