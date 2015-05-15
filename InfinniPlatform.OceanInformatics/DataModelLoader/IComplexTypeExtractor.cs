using System.Collections.Generic;
using InfinniPlatform.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public interface IComplexTypeExtractor
    {
        IEnumerable<ComplexTypeModel> ExtractComplexTypeModels();
    }
}