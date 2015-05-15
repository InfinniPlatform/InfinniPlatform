using InfinniPlatform.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public interface ITemplateExtractor
    {
        DocumentModel Extract(string oetFilePath, string templatesFolder, string archetypesFolder);
    }
}