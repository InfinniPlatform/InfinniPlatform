using System.Collections.Generic;

using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Core.ModelRepository
{
    public interface IComplexTypeExtractor
    {
        IDictionary<string, DataSchema> ExtractComplexTypeModels();
    }
}