using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.ModelRepository;
using InfinniPlatform.Core.ModelRepository.DataConverters;
using InfinniPlatform.OceanInformatics.DataModelLoader;

namespace InfinniPlatform.ModelRepository.OpenEhrDataConverter
{
    public sealed class OceanOpenEhrFactory : IOpenEhrFactory
    {
        public IDataConverter BuildDataConverter()
        {
            return new OceanOpenEhrDataConverter();
        }

        public IArchetypeExtractor BuildArchetypeExtractor()
        {
            return new ArchetypeExtractor("C74887E5-D039-4215-977C-05BC827B6585");
        }

        public IComplexTypeExtractor BuildComplexTypeExtractor()
        {
            return new ComplexTypeExtractor("C74887E5-D039-4215-977C-05BC827B6585");
        }

        public ITemplateExtractor BuildTemplateExtractor()
        {
            return new TemplateExtractor("C74887E5-D039-4215-977C-05BC827B6585");
        }
    }
}
