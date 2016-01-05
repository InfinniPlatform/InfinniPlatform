using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Core.ModelRepository
{
    public interface IArchetypeExtractor
    {
        DataSchema Extract(string architypePath);
    }
}