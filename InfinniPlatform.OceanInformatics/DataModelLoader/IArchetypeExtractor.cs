using InfinniPlatform.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public interface IArchetypeExtractor
    {
        ArchetypeModel Extract(string architypePath);
    }
}