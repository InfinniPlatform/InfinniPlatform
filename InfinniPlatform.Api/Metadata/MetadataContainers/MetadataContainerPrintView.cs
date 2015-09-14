namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
    public sealed class MetadataContainerPrintView : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.PrintViewContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.PrintView;
        }
    }
}