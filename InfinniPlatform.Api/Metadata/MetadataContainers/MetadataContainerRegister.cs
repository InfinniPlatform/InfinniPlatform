namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
    public sealed class MetadataContainerRegister : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.RegisterContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.Register;
        }
    }
}