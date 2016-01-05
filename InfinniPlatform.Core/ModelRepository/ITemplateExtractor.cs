using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Core.ModelRepository
{
    public interface ITemplateExtractor
    {
        DataSchema Extract(string oetFilePath, string templatesFolder, string archetypesFolder);
    }
}