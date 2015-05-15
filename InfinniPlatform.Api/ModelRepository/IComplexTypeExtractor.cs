using System.Collections.Generic;
using InfinniPlatform.Api.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.Api.ModelRepository
{
    public interface IComplexTypeExtractor
    {
        IDictionary<string, DataSchema> ExtractComplexTypeModels();
    }
}