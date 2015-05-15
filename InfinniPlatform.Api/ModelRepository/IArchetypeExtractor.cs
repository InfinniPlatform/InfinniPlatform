using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Api.ModelRepository
{
    public interface IArchetypeExtractor
    {
        DataSchema Extract(string architypePath);
    }
}