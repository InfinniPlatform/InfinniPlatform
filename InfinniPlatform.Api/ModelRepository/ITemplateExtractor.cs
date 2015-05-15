using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Api.ModelRepository
{
    public interface ITemplateExtractor
    {
        DataSchema Extract(string oetFilePath, string templatesFolder, string archetypesFolder);
    }
}