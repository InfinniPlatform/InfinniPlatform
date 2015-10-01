namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
    public sealed class MetadataContainerReport : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.ReportContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.Report;
        }
    }
}