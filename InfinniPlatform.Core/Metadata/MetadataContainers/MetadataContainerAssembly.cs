namespace InfinniPlatform.Core.Metadata.MetadataContainers
{
    public sealed class MetadataContainerAssembly : IMetadataContainerInfo
    {
        public string GetMetadataContainerName()
        {
            return MetadataType.AssemblyContainer;
        }

        public string GetMetadataTypeName()
        {
            return MetadataType.Assembly;
        }
    }
}